using PTMKTestTaskConsole.BusinessLogic;
using PTMKTestTaskConsole.DataAccess;

if (args.Length == 0)
{
    Console.WriteLine("Введите режим работы приложения (1 - 5).");
    return;
}

var service = new EmployeeService();

int mode = int.Parse(args[0]);
if(mode != 1)
{
    using (var dbContext = new DatabaseContext())
    {
        if (!dbContext.DatabaseExists())
        {
            Console.WriteLine("Ошибка: база данных или таблица сотрудников не найдены. Сначала создайте таблицу (режим 1).");
            return;
        }
    }
}
switch (mode)
{
    case 1:
        service.CreateEmployeeTable();
        Console.WriteLine("Таблица сотрудников создана.");
        break;
    case 2:
        if (args.Length < 4)
        {
            Console.WriteLine("Пример ввода: myApp 2 \"Ivanov Petr Sergeevich\" 2009-07-12 Male");
            return;
        }

        string fullName = string.Join(" ", args.Skip(1).Take(args.Length - 3));
        DateOnly birthDate;
        string gender = args[^1];  

        if (!DateOnly.TryParse(args[^2], out birthDate))
        {
            Console.WriteLine("Некорректная дата. Пример: 2009-07-12");
            return;
        }

        if (!string.Equals(gender, "Male", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(gender, "Female", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Некорректное значение пола. Допустимые значения: Male, Female.");
            return;
        }

        service.AddEmployee(fullName, birthDate, gender);
        Console.WriteLine("Сотрудник добавлен.");
        break;
    case 3:
        service.ListEmployees();
        break;
    case 4:
        service.BulkInsertEmployees(1000000);
        break;
    case 5:
        service.ListEmployeesByCriteria();
        break;
    default:
        Console.WriteLine("Ошибка выбора режима работы.");
        break;
}
        