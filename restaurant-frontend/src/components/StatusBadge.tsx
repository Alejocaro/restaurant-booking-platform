interface Props { label: string; color?: string }

const COLORS: Record<string, string> = {
  green: 'bg-green-100 text-green-800',
  yellow: 'bg-yellow-100 text-yellow-800',
  red: 'bg-red-100 text-red-800',
  blue: 'bg-blue-100 text-blue-800',
  gray: 'bg-gray-100 text-gray-700',
};

export default function StatusBadge({ label, color = 'gray' }: Props) {
  return (
    <span className={`px-2 py-0.5 rounded-full text-xs font-medium ${COLORS[color] ?? COLORS.gray}`}>
      {label}
    </span>
  );
}
