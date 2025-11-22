class Person {
    private String name;
    private int age;
    private String city;
    
    public Person(String name, int age, String city) {
        this.name = name;
        this.age = age;
        this.city = city;
    }
    
    public String greet() {
        return "Hello, my name is " + name;
    }
    
    public String getInfo() {
        return name + " is " + age + " years old and lives in " + city;
    }
}

public class first {

    public static void main(String[] aegs) {
        
        Person person1 = new Person("David", 52, "Sapulpa OK");
        Person person2 = new Person("Bob", 30, "Los Angeles");
        
        // Use Person methods
        System.out.println(person1.greet());
        System.out.println(person1.getInfo());
        
        System.out.println(person2.greet());
        System.out.println(person2.getInfo());
    }
} 