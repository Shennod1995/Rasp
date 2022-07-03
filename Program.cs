public static void CreateNewObjectOnMap()
{
    //Создание объекта на карте
}

public static void SetRandomChance()
{
    _chance = Random.Range(0, 100);
}

public static int GetSalary(int hoursWorked)
{
    return _hourlyRate * hoursWorked;
}