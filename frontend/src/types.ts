export type Member = {
  id: string;
  fullName: string;
  phone: string;
  monthlyFee: number;
  nextDueDate: string;
  isActive: boolean;
  notes?: string | null;
};

export type PaymentMethod = 'Cash' | 'Transfer' | 'MercadoPago';
