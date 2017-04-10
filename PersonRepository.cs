public class PersonRepository : Repository<TablePerson>
{
    public PersonRepository() : base("[TablePerson]"
                                      , @"[IdPerson]
                                      , [Name]
                                      , [Nickname]"
                                      , "IdPerson") { }
    

    /*
        comment
     */

    public bool InsertPerson(TablePerson person)
    {
        string sql = string.Format(@"INSERT INTO [dbo].[TablePerson]
                                        ([Name]
                                        ,[Nickname])
                                        VALUES
                                            ('{0}'
                                            ,'{1}')", person.Name
                                                    , person.Nickname);
        return this.InsertOrUpdate(sql);
    }

    public bool UpdatePerson(TablePerson person)
    {
        string sql = string.Format(@"UPDATE [dbo].[TablePerson]
                                         SET [Name] = '{0}'      
                                            ,[Nickname] = '{1}'
                                         WHERE Id = {1}", person.Name
                                                        , person.Nickname
                                                        , person.Id);
        return this.InsertOrUpdate(sql);
    }

    /*
       comment 
     */
    public List<TablePerson> FindAllPerson() {
        return (List<TablePerson>)this.FindAll();
    }

    /**
        comment
     */
    public TablePerson FindByName(string name) {
        string whereClause = String.Format(@"WHERE Name = {0}", name);

        return (TablePerson)this.FindByColumn(whereClause);
    }

    /*
        comment
     */
    public TablePerson FindPerson(string sql) {
        return (TablePerson)this.Find(sql);
    }

    /*
        comment
     */
    public bool RemovePerson(int id)
    {
        return this.Remove(id);
    }
}