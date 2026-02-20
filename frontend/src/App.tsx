import { useEffect, useMemo, useState } from 'react';
import { fetchMembers, markMemberAsPaid } from './api';
import type { Member } from './types';

type TabKey = 'overdue' | 'dueSoon' | 'all';

const tabToFilter: Record<TabKey, string | undefined> = {
  overdue: 'overdue',
  dueSoon: 'dueSoon',
  all: undefined
};

export function App() {
  const [tab, setTab] = useState<TabKey>('overdue');
  const [members, setMembers] = useState<Member[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const title = useMemo(() => {
    if (tab === 'overdue') return 'Vencidos';
    if (tab === 'dueSoon') return 'Por vencer';
    return 'Todos';
  }, [tab]);

  async function load() {
    try {
      setLoading(true);
      setError(null);
      const data = await fetchMembers(tabToFilter[tab], 7);
      setMembers(data);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    load();
  }, [tab]);

  async function onMarkPaid(member: Member) {
    await markMemberAsPaid(member.id, member.monthlyFee, 'Cash');
    await load();
  }

  return (
    <main className="container">
      <h1>Gym Billing</h1>
      <p>SaaS simple para control de cuotas en gimnasios.</p>

      <div className="tabs">
        <button onClick={() => setTab('overdue')} className={tab === 'overdue' ? 'active' : ''}>Vencidos</button>
        <button onClick={() => setTab('dueSoon')} className={tab === 'dueSoon' ? 'active' : ''}>Por vencer</button>
        <button onClick={() => setTab('all')} className={tab === 'all' ? 'active' : ''}>Todos</button>
      </div>

      <h2>{title}</h2>

      {loading && <p>Cargando...</p>}
      {error && <p className="error">{error}</p>}

      <ul className="list">
        {members.map((member) => {
          const whatsappUrl = `https://wa.me/${member.phone}?text=${encodeURIComponent(
            `Hola ${member.fullName}, te recordamos que tu cuota vence el ${member.nextDueDate}.`
          )}`;

          return (
            <li key={member.id}>
              <div>
                <strong>{member.fullName}</strong>
                <p>Vence: {member.nextDueDate}</p>
                <p>Cuota: ${member.monthlyFee.toLocaleString('es-AR')}</p>
              </div>
              <div className="actions">
                <button onClick={() => onMarkPaid(member)}>Marcar pagado</button>
                <a href={whatsappUrl} target="_blank" rel="noreferrer">WhatsApp</a>
              </div>
            </li>
          );
        })}
      </ul>
    </main>
  );
}
