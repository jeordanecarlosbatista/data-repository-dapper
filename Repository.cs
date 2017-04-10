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
        Mapeando propriedades da tabela como Nome, Colunas e PrimaryKey.
    */
    protected Repository(string tableName, string columnsMapping, string primaryKeyName)
    {
        this.tableName = tableName;
        this.columnsMapping = columnsMapping;
        this.primaryKeyName = primaryKeyName;
    }

    /*
        Consulta que retorna todos os registros da tabela.
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
        Consulta que retorna todos os registros da tabela. 

        @param sql - SQL query
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
        Conulta que retorna um único registro da tabela.
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
        Consulta que retorna um único registro da tabela.

        @param whereClause - Condição da consulta passada como parâmetro.
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
        Método responsável por gravar ou atualizar dados na tabela.

        @param command - SQL query (INSERT ou UPDATE) passado como parâmetro.
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
        Método responsável por remover um registro da tabela.

        @param id - Id do registro que será apagado.
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