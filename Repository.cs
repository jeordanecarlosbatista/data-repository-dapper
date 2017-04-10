public abstract class Repository<T>
{
    private readonly string tableName;
    private readonly string columnsMapping;
    private readonly string primaryKeyName;

    interval IDbConnection Connection
    {
        get 
        {
            return new SQLConncetion(ConfigurationManager.ConnectionStrings["KeyName"].ConnectionString)
        }
    }

    /*
        comment
    */
    protected Repository(string tableName, string columnsMapping, string primaryKeyName)
    {
        this.tableName = tableName;
        this.columnsMapping = columnsMapping;
        this.primaryKeyName = primaryKeyName;
    }

    /*
        comment
    */
    protected IEnumerable<T> FindAll()
    {
        IEnumerable<T> items = null;

        using (IDbConnection cn = Connection)
        {
            cn.Open();
            items = cn.Query<T>(string.Format(@"SELECT {0} FROM {1}", columnsMapping, tableName));
        }
        return items;
    }

    /*
        comment
    */
    protected IEnumerable<T> FindAll(string sql)
    {
        IEnumerable<T> items = null;

        using (IDbConnection cn = Connection)
        {
            cn.Open();
            items = cn.Query<T>(sql);
        }
        return items;
    }

    /*
        comment
    */
    protected T Find(string sql)
    {
        T item = default(T);

        using (IDbConnection cn = Connection)
        {
            cn.Open();
            item = cn.Query<T>(sql).SingleOrDefault();
        }
        return item;
    }

    /*
        comment
    */
    protected T FindByColumn(string whereClause)
    {
        T item = default(T);
        using (IDbConnection cn = Connection)
        {
            cn.Open();
            item = cn.Query<T>(string.Format(@"SELECT {0} 
                                                FROM [dbo].[{1}] 
                                                WHERE 1 = 1
                                                {2}", columnsMapping,
                                                        tableName,
                                                        whereClause)).SingleOrDefault();
        }
        return item;
    }

    /*
        comment
    */
    protected bool InsertOrUpdate(string command)
    {
        using (IDbConnection cn = Connection)
        {
            cn.Open();
            return cn.Execute(command) == 1;
        }
    }

    /*
        comment
    */
    protected bool Remove(int id)
    {
        using (IDbConnection cn = Connection)
        {
            cn.Open();
            return cn.Execute(string.Format(@"DELETE FROM [dbo].[{0}] 
                                                WHERE {1} = {2} ",
                                                tableName,
                                                primaryKeyName,
                                                id)) == 1;
        }
    }
}