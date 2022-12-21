export function addMonths(date: Date, amountOfMonth: number): Date{
    return new Date(date.setMonth(date.getMonth() + amountOfMonth));
} 