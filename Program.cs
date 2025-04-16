using System;
//Интерфейс для всех фигур.
public interface IShape 
{
    double Area();
    void PrintInfo();
}
//Абстрактный класс Фигура.
public abstract class Shape : IShape
{
    private string _name;
    public string Name 
    {
        get => _name;
        private set => _name = !string.IsNullOrWhiteSpace(value) 
            ? value 
            : throw new ArgumentException("Название фигуры не может быть пустым");
    }
    //Абстрактный метод для вычисления площади.
    public abstract double Area();
    //Абстрактный метод для вывода информации.
    public virtual void PrintInfo()
    {
        Console.WriteLine($"Фигура: {Name}");
    }
    //Конструктор класса.
    protected Shape(string name)
    {
        Name = name;
        Console.WriteLine($"Создана фигура: {Name}");
    }
    //Деструктор.
    ~Shape()
    {
        Console.WriteLine($"Фигура {Name} уничтожена");
    }
}
//Абстрактный класс Четырёхугольник.
public abstract class Quadrilateral : Shape
{
    private double _sideA, _sideB, _sideC, _sideD;
    public double SideA { get => _sideA; protected set => _sideA = ValidateSide(value); }
    public double SideB { get => _sideB; protected set => _sideB = ValidateSide(value); }
    public double SideC { get => _sideC; protected set => _sideC = ValidateSide(value); }
    public double SideD { get => _sideD; protected set => _sideD = ValidateSide(value); }
    //Конструктор класса.
    protected Quadrilateral(string name, double a, double b, double c, double d) : base(name)
    {
        SideA = a;
        SideB = b;
        SideC = c;
        SideD = d;   
    }
    private double ValidateSide(double value) => value > 0 ? value : throw new ArgumentException("Длина стороны должна быть > 0");
    //Вывод информации о сторонах и вызов его базовой реализации.
    public override void PrintInfo()
    {
        base.PrintInfo();
        Console.WriteLine($"Стороны: A={SideA:F2}, B={SideB:F2}, C={SideC:F2}, D={SideD:F2}");
    }
}
//Класс Квадрат.
public class Square : Quadrilateral
{
    public Square(double side) : base("Квадрат", side, side, side, side) 
    {
        ValidateSquare();
    }
    private void ValidateSquare()
    {
        const double tolerance = 0.0001;
        if (Math.Abs(SideA - SideB) > tolerance || 
            Math.Abs(SideA - SideC) > tolerance ||
            Math.Abs(SideA - SideD) > tolerance)
        {
            throw new ArgumentException("Все стороны квадрата должны быть равны");
        }
    }
    //Переопределение метода.
    public override double Area()
    {
        try 
        {
            return checked(SideA * SideA);
        }
        catch (OverflowException)
        {
            throw new OverflowException("Площадь слишком большая");
        }
    }
}
//Класс Прямоугольник.
public class Rectangle : Quadrilateral
{
    //Конструктор класса.
    public Rectangle(double a, double b) : base("Прямоугольник", a, b, a, b)
    {
        ValidateRectangle();
    }
    private void ValidateRectangle()
    {
        const double tolerance = 0.0001;
        if (Math.Abs(SideA - SideC) > tolerance || 
            Math.Abs(SideB - SideD) > tolerance)
        {
            throw new ArgumentException("Противоположные стороны прямоугольника должны быть равны");
        }
    }
    //Переопределение метода. 
    public override double Area()
    {
        try 
        {
            return checked(SideA * SideB);
        }
        catch (OverflowException)
        {
            throw new OverflowException("Площадь слишком большая");
        }
    }
}
//Абстрактный класс Треугольник.
public abstract class Triangle : Shape
{
    private double _sideA, _sideB, _sideC;
    public double SideA { get => _sideA; protected set => _sideA = ValidateSide(value); }
    public double SideB { get => _sideB; protected set => _sideB = ValidateSide(value); }
    public double SideC { get => _sideC; protected set => _sideC = ValidateSide(value); }
    //Конструктор класса.
    protected Triangle(string name, double a, double b, double c) : base(name)
    {
        SideA = a;
        SideB = b;
        SideC = c;
        ValidateTriangle();
    }
    private double ValidateSide(double value) => 
        value > 0 ? value : throw new ArgumentException("Длина стороны должна быть > 0");
    private void ValidateTriangle()
    {
        if (!IsValidTriangle(SideA, SideB, SideC))
            throw new ArgumentException("Недопустимые длины сторон треугольника");
    }
    private static bool IsValidTriangle(double a, double b, double c) => 
        a + b > c && a + c > b && b + c > a;
    //Переопределение метода.
    public override void PrintInfo()
    {
        base.PrintInfo();
        Console.WriteLine($"Стороны: A={SideA:F2}, B={SideB:F2}, C={SideC:F2}");
    }
}
//Класс Равнобедренный треугольник.
public class IsoscelesTriangle : Triangle
{
    public IsoscelesTriangle(double a, double b) : base("Равнобедренный треугольник", a, a, b)
    {
        ValidateIsosceles();
    }
    private void ValidateIsosceles()
    {
        const double tolerance = 0.0001;
        if (Math.Abs(SideA - SideB) > tolerance)
            throw new ArgumentException("Две стороны должны быть равны");
    }
    //Переопределение метода.
    public override double Area()
    {
        try 
        {
            double h = Math.Sqrt(SideA * SideA - (SideC * SideC / 4.0));
            return checked((SideC * h) / 2);
        }
        catch (OverflowException)
        {
            throw new OverflowException("Площадь слишком большая");
        }
    }
}
//Класс Прямоугольный треугольник.
public class RightTriangle : Triangle
{
    public RightTriangle(double a, double b) : base("Прямоугольный треугольник", a, b, Math.Sqrt(a * a + b * b))
    {
        ValidateRightTriangle();
    }
    private void ValidateRightTriangle()
    {
        const double tolerance = 0.0001;
        double expectedHypotenuse = Math.Sqrt(SideA * SideA + SideB * SideB);
        if (Math.Abs(SideC - expectedHypotenuse) > tolerance)
            throw new ArgumentException("Несоответствие теореме Пифагора");
    }
    //Переопределение метода.
    public override double Area()
    {
        try 
        {
            return checked((SideA * SideB) / 2);
        }
        catch (OverflowException)
        {
            throw new OverflowException("Площадь слишком большая");
        }
    }
}
//Класс Равносторонний треугольник.
public class EquilateralTriangle : Triangle
{
    public EquilateralTriangle(double a) : base("Равносторонний треугольник", a, a, a)
    {
        ValidateEquilateral();
    }
    private void ValidateEquilateral()
    {
        const double tolerance = 0.0001;
        if (Math.Abs(SideA - SideB) > tolerance || 
            Math.Abs(SideA - SideC) > tolerance)
            throw new ArgumentException("Все стороны должны быть равны");
    }
    //Переопределение метода.
    public override double Area()
    {
        try 
        {
            return checked(Math.Sqrt(3) / 4.0 * SideA * SideA);
        }
        catch (OverflowException)
        {
            throw new OverflowException("Площадь слишком большая");
        }
    }
}
//Входные данные записываем сами.
class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Введите данные для фигур:");
            //Ввод данных для квадрата.
            Console.Write("Сторона квадрата: ");
            double squareSide = double.Parse(Console.ReadLine());
            //Ввод данных для прямоугольника.
            Console.Write("Сторона A прямоугольника: ");
            double rectA = double.Parse(Console.ReadLine());
            Console.Write("Сторона B прямоугольника: ");
            double rectB = double.Parse(Console.ReadLine());
            //Ввод данных для равнобедренного треугольника.
            Console.Write("Боковая сторона равнобедренного треугольника: ");
            double isoA = double.Parse(Console.ReadLine());
            Console.Write("Основание равнобедренного треугольника: ");
            double isoB = double.Parse(Console.ReadLine());
            //Ввод данных для прямоугольного треугольника.
            Console.Write("Катет A прямоугольного треугольника: ");
            double rightA = double.Parse(Console.ReadLine());
            Console.Write("Катет B прямоугольного треугольника: ");
            double rightB = double.Parse(Console.ReadLine());
            //Ввод данных для равностороннего треугольника.
            Console.Write("Сторона равностороннего треугольника: ");
            double equiSide = double.Parse(Console.ReadLine());
            //Создание массива объектов.
            IShape[] shapes = new IShape[]
            {
                new Square(squareSide),
                new Rectangle(rectA, rectB),
                new IsoscelesTriangle(isoA, isoB),
                new RightTriangle(rightA, rightB),
                new EquilateralTriangle(equiSide)
            };
            //Вывод информации.
            Console.WriteLine("\nРезультаты:");
            foreach (var shape in shapes)
            {
                shape.PrintInfo();
                Console.WriteLine($"Площадь: {shape.Area():F2}");
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
