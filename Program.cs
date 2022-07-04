private void CheckButton_Click(object sender, EventArgs e)
{
    string passportNumber = GetPassportNumber();
    string commandText = string.Format("select * from passports where num='{0}' limit 1;", (object)Form1.ComputeSha256Hash(passportNumber));
    DatabaseLocalReader databaseReader = new DatabaseLocalReader(commandText);
    DataTable resault = databaseReader.SendReuqest();
    ShowResault(resault);
}

private static string GetPassportNumber()
{
    string passportNumber = passportTextbox.Text.Trim();
    if (string.IsNullOrEmpty(passportNumber))
    {
        MessageBox.Show("Введите серию и номер паспорта");
    }

    if (passportNumber.Length < 10)
    {
        MessageBox.Show("Неверный формат серии или номера паспорта");
    }

    return passportNumber.Replace(" ", string.Empty);
}

private static object ComputeSha256Hash(string passportNumber)
{
    return passportNumber;
}

private static void ShowResault(DataTable dataTable)
{
    string passportNumber = GetPassportNumber();

    if (dataTable.Rows.Count == 0)
    {
        textResult.Text = $"Паспорт «{passportNumber}» в списке участников дистанционного голосования НЕ НАЙДЕН";
        return;
    }

    DataRow voterData = dataTable.Rows[0];
    bool accessVoter = Convert.ToBoolean(voterData.ItemArray[1]);
    if (accessVoter)
    {
        textResult.Text = $"По паспорту «{passportNumber}» доступ к бюллетеню на дистанционном электронном голосовании ПРЕДОСТАВЛЕН";
    }
    else
    {
        textResult.Text = $"По паспорту «{passportNumber}» доступ к бюллетеню на дистанционном электронном голосовании НЕ ПРЕДОСТАВЛЯЛСЯ";
    }
}

    public class DatabaseLocalReader
{
    private readonly string _commandText;
    private readonly string _pathToDatabase;
    private const string _databaseName = "db.db";
    private readonly SQLiteConnection _connection;
    private DataTable _dataTable;

    private string PathDB => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    public DatabaseLocalReader(string commandText)
    {
        _commandText = commandText;
        _pathToDatabase = $"Data Source={PathDB}\\{_databaseName}";
        _connection = new SQLiteConnection(_pathToDatabase);
    }

    public DataTable SendReuqest()
    {
        try
        {
            TrySQLiteConnect();
        }
        catch (SQLiteException ex)
        {
            MessageBoxError(ex);
        }

        return _dataTable;
    }

    private void TrySQLiteConnect()
    {
        _connection.Open();
        SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(new SQLiteCommand(_commandText, _connection));
        _dataTable = new DataTable();
        sqLiteDataAdapter.Fill(_dataTable);
        _connection.Close();
    }

    private void MessageBoxError(SQLiteException ex)
    {
        if (ex.ErrorCode == 1)
        {
            int num2 = (int)MessageBox.Show("Файл db.sqlite не найден. Положите файл в папку вместе с exe.");
        }
    }
}