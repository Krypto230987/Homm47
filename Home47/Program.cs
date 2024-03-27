using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home47
{

    [AttributeUsage(AttributeTargets.Property)]
    public class MyPropertyInfoAttribute : Attribute
    {
        public string SerializationName { get; set; }
    }


    class MySerializer<T>
    {
        private T obj;

        public MySerializer(T obj)
        {
            this.obj = obj;
        }

        public string Serialize()
        {
            Type type = typeof(T);
            System.Reflection.PropertyInfo[] properties = type.GetProperties();
            string result = "";

            foreach (System.Reflection.PropertyInfo property in properties)
            {
                object[] attributes = property.GetCustomAttributes(typeof(MyPropertyInfoAttribute), false);
                if (attributes.Length > 0)
                {
                    MyPropertyInfoAttribute attribute = (MyPropertyInfoAttribute)attributes[0];
                    result += $"{attribute.SerializationName}: {property.GetValue(obj)}\n";
                }
            }

            return result;
        }
    }


    class Employee
    {
        [MyPropertyInfo(SerializationName = "Имя сотрудника")]
        public string Name { get; set; }

        [MyPropertyInfo(SerializationName = "Опыт работы")]
        public int Experience { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Employee employee = new Employee { Name = "Ivanov Ivan", Experience = 5 };
            MySerializer<Employee> employeeSerializer = new MySerializer<Employee>(employee);
            string serializedEmployee = employeeSerializer.Serialize();

            Console.WriteLine(serializedEmployee);
            Console.ReadKey();
        }
    }
}
