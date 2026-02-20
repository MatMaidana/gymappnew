import type { Member, PaymentMethod } from './types';

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5000/api';

export async function fetchMembers(filter?: string, dueSoonDays = 7): Promise<Member[]> {
  const query = new URLSearchParams();
  if (filter) query.set('filter', filter);
  if (filter === 'dueSoon') query.set('dueSoonDays', String(dueSoonDays));

  const response = await fetch(`${apiBaseUrl}/members?${query.toString()}`);
  if (!response.ok) throw new Error('No se pudo cargar alumnos');
  return response.json();
}

export async function markMemberAsPaid(memberId: string, amount: number, method: PaymentMethod): Promise<void> {
  const now = new Date();
  const period = `${now.getUTCFullYear()}-${String(now.getUTCMonth() + 1).padStart(2, '0')}`;

  const response = await fetch(`${apiBaseUrl}/members/${memberId}/payments`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      amount,
      paidAt: now.toISOString(),
      method,
      period
    })
  });

  if (!response.ok) throw new Error('No se pudo registrar pago');
}
