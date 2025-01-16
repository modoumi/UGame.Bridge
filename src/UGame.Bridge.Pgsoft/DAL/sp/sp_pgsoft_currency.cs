/******************************************************
 * 此代码由代码生成器工具自动生成，请不要修改
 * AiUo代码生成器核心库版本号：1.0.0.0
 * git: https://github.com/jh98net/AiUo
 * 文档生成时间：2023-07-11 16: 48:48
 ******************************************************/
using System;
using System.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AiUo;
using AiUo.Data;
using MySql.Data.MySqlClient;
using System.Text;
using AiUo.Data.MySql;

namespace UGame.Bridge.Pgsoft.DAL
{
	#region EO
	/// <summary>
	/// 提供商货币基础单位
	/// 【表 sp_pgsoft_currency 的实体类】
	/// </summary>
	[DataContract]
	public class Sp_pgsoft_currencyEO : IRowMapper<Sp_pgsoft_currencyEO>
	{
		/// <summary>
		/// 构造函数 
		/// </summary>
		public Sp_pgsoft_currencyEO()
		{
			this.BaseUnit = 0;
		}
		#region 主键原始值 & 主键集合
	    
		/// <summary>
		/// 当前对象是否保存原始主键值，当修改了主键值时为 true
		/// </summary>
		public bool HasOriginal { get; protected set; }
		
		private string _originalCurrencyID;
		/// <summary>
		/// 【数据库中的原始主键 CurrencyID 值的副本，用于主键值更新】
		/// </summary>
		public string OriginalCurrencyID
		{
			get { return _originalCurrencyID; }
			set { HasOriginal = true; _originalCurrencyID = value; }
		}
	    /// <summary>
	    /// 获取主键集合。key: 数据库字段名称, value: 主键值
	    /// </summary>
	    /// <returns></returns>
	    public Dictionary<string, object> GetPrimaryKeys()
	    {
	        return new Dictionary<string, object>() { { "CurrencyID", CurrencyID }, };
	    }
	    /// <summary>
	    /// 获取主键集合JSON格式
	    /// </summary>
	    /// <returns></returns>
	    public string GetPrimaryKeysJson() => SerializerUtil.SerializeJson(GetPrimaryKeys());
		#endregion // 主键原始值
		#region 所有字段
		/// <summary>
		/// 货币类型
		/// 【主键 varchar(20)】
		/// </summary>
		[DataMember(Order = 1)]
		public string CurrencyID { get; set; }
		/// <summary>
		/// 对方currencyId
		/// 【字段 varchar(20)】
		/// </summary>
		[DataMember(Order = 2)]
		public string TrdCurrencyID { get; set; }
		/// <summary>
		/// 货币名称
		/// 【字段 varchar(50)】
		/// </summary>
		[DataMember(Order = 3)]
		public string Name { get; set; }
		/// <summary>
		/// 基础单位
		/// 【字段 int】
		/// </summary>
		[DataMember(Order = 4)]
		public int BaseUnit { get; set; }
		#endregion // 所有列
		#region 实体映射
		
		/// <summary>
		/// 将IDataReader映射成实体对象
		/// </summary>
		/// <param name = "reader">只进结果集流</param>
		/// <return>实体对象</return>
		public Sp_pgsoft_currencyEO MapRow(IDataReader reader)
		{
			return MapDataReader(reader);
		}
		
		/// <summary>
		/// 将IDataReader映射成实体对象
		/// </summary>
		/// <param name = "reader">只进结果集流</param>
		/// <return>实体对象</return>
		public static Sp_pgsoft_currencyEO MapDataReader(IDataReader reader)
		{
		    Sp_pgsoft_currencyEO ret = new Sp_pgsoft_currencyEO();
			ret.CurrencyID = reader.ToString("CurrencyID");
			ret.OriginalCurrencyID = ret.CurrencyID;
			ret.TrdCurrencyID = reader.ToString("TrdCurrencyID");
			ret.Name = reader.ToString("Name");
			ret.BaseUnit = reader.ToInt32("BaseUnit");
		    return ret;
		}
		
		#endregion
	}
	#endregion // EO

	#region MO
	/// <summary>
	/// 提供商货币基础单位
	/// 【表 sp_pgsoft_currency 的操作类】
	/// </summary>
	public class Sp_pgsoft_currencyMO : MySqlTableMO<Sp_pgsoft_currencyEO>
	{
		/// <summary>
		/// 表名
		/// </summary>
	    public override string TableName { get; set; } = "`sp_pgsoft_currency`";
	    
		#region Constructors
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name = "database">数据来源</param>
		public Sp_pgsoft_currencyMO(MySqlDatabase database) : base(database) { }
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name = "connectionStringName">配置文件.config中定义的连接字符串名称</param>
		public Sp_pgsoft_currencyMO(string connectionStringName = null) : base(connectionStringName) { }
	    /// <summary>
	    /// 构造函数
	    /// </summary>
	    /// <param name="connectionString">数据库连接字符串，如server=192.168.1.1;database=testdb;uid=root;pwd=root</param>
	    /// <param name="commandTimeout">CommandTimeout时间</param>
	    public Sp_pgsoft_currencyMO(string connectionString, int commandTimeout) : base(connectionString, commandTimeout) { }
		#endregion // Constructors
	    
	    #region  Add
		/// <summary>
		/// 插入数据
		/// </summary>
		/// <param name = "item">要插入的实体对象</param>
		/// <param name="tm_">事务管理对象</param>
		/// <param name="useIgnore_">是否使用INSERT IGNORE</param>
		/// <return>受影响的行数</return>
		public override int Add(Sp_pgsoft_currencyEO item, TransactionManager tm_ = null, bool useIgnore_ = false)
		{
			RepairAddData(item, useIgnore_, out string sql_, out List<MySqlParameter> paras_);
			return Database.ExecSqlNonQuery(sql_, paras_, tm_); 
		}
		public override async Task<int> AddAsync(Sp_pgsoft_currencyEO item, TransactionManager tm_ = null, bool useIgnore_ = false)
		{
			RepairAddData(item, useIgnore_, out string sql_, out List<MySqlParameter> paras_);
			return await Database.ExecSqlNonQueryAsync(sql_, paras_, tm_); 
		}
	    private void RepairAddData(Sp_pgsoft_currencyEO item, bool useIgnore_, out string sql_, out List<MySqlParameter> paras_)
		{
			sql_ = useIgnore_ ? "INSERT IGNORE" : "INSERT";
			sql_ += $" INTO {TableName} (`CurrencyID`, `TrdCurrencyID`, `Name`, `BaseUnit`) VALUE (@CurrencyID, @TrdCurrencyID, @Name, @BaseUnit);";
			paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@CurrencyID", item.CurrencyID, MySqlDbType.VarChar),
				Database.CreateInParameter("@TrdCurrencyID", item.TrdCurrencyID != null ? item.TrdCurrencyID : (object)DBNull.Value, MySqlDbType.VarChar),
				Database.CreateInParameter("@Name", item.Name != null ? item.Name : (object)DBNull.Value, MySqlDbType.VarChar),
				Database.CreateInParameter("@BaseUnit", item.BaseUnit, MySqlDbType.Int32),
			};
		}
		public int AddByBatch(IEnumerable<Sp_pgsoft_currencyEO> items, int batchCount, TransactionManager tm_ = null)
		{
			var ret = 0;
			foreach (var sql in BuildAddBatchSql(items, batchCount))
			{
				ret += Database.ExecSqlNonQuery(sql, tm_);
	        }
			return ret;
		}
	    public async Task<int> AddByBatchAsync(IEnumerable<Sp_pgsoft_currencyEO> items, int batchCount, TransactionManager tm_ = null)
	    {
	        var ret = 0;
	        foreach (var sql in BuildAddBatchSql(items, batchCount))
	        {
	            ret += await Database.ExecSqlNonQueryAsync(sql, tm_);
	        }
	        return ret;
	    }
	    private IEnumerable<string> BuildAddBatchSql(IEnumerable<Sp_pgsoft_currencyEO> items, int batchCount)
		{
			var count = 0;
	        var insertSql = $"INSERT INTO {TableName} (`CurrencyID`, `TrdCurrencyID`, `Name`, `BaseUnit`) VALUES ";
			var sql = new StringBuilder();
	        foreach (var item in items)
			{
				count++;
				sql.Append($"('{item.CurrencyID}','{item.TrdCurrencyID}','{item.Name}',{item.BaseUnit}),");
				if (count == batchCount)
				{
					count = 0;
					sql.Insert(0, insertSql);
					var ret = sql.ToString().TrimEnd(',');
					sql.Clear();
	                yield return ret;
				}
			}
			if (sql.Length > 0)
			{
	            sql.Insert(0, insertSql);
	            yield return sql.ToString().TrimEnd(',');
	        }
	    }
	    #endregion // Add
	    
		#region Remove
		#region RemoveByPK
		/// <summary>
		/// 按主键删除
		/// </summary>
		/// /// <param name = "currencyID">货币类型</param>
		/// <param name="tm_">事务管理对象</param>
		public int RemoveByPK(string currencyID, TransactionManager tm_ = null)
		{
			RepiarRemoveByPKData(currencyID, out string sql_, out List<MySqlParameter> paras_);
			return Database.ExecSqlNonQuery(sql_, paras_, tm_);
		}
		public async Task<int> RemoveByPKAsync(string currencyID, TransactionManager tm_ = null)
		{
			RepiarRemoveByPKData(currencyID, out string sql_, out List<MySqlParameter> paras_);
			return await Database.ExecSqlNonQueryAsync(sql_, paras_, tm_);
		}
		private void RepiarRemoveByPKData(string currencyID, out string sql_, out List<MySqlParameter> paras_)
		{
			sql_ = $"DELETE FROM {TableName} WHERE `CurrencyID` = @CurrencyID";
			paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
			};
		}
		/// <summary>
		/// 删除指定实体对应的记录
		/// </summary>
		/// <param name = "item">要删除的实体</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>受影响的行数</return>
		public int Remove(Sp_pgsoft_currencyEO item, TransactionManager tm_ = null)
		{
			return RemoveByPK(item.CurrencyID, tm_);
		}
		public async Task<int> RemoveAsync(Sp_pgsoft_currencyEO item, TransactionManager tm_ = null)
		{
			return await RemoveByPKAsync(item.CurrencyID, tm_);
		}
		#endregion // RemoveByPK
		
		#region RemoveByXXX
		#region RemoveByTrdCurrencyID
		/// <summary>
		/// 按字段删除
		/// </summary>
		/// /// <param name = "trdCurrencyID">对方currencyId</param>
		/// <param name="tm_">事务管理对象</param>
		public int RemoveByTrdCurrencyID(string trdCurrencyID, TransactionManager tm_ = null)
		{
			RepairRemoveByTrdCurrencyIDData(trdCurrencyID, out string sql_, out List<MySqlParameter> paras_);
			return Database.ExecSqlNonQuery(sql_, paras_, tm_);
		}
		public async Task<int> RemoveByTrdCurrencyIDAsync(string trdCurrencyID, TransactionManager tm_ = null)
		{
			RepairRemoveByTrdCurrencyIDData(trdCurrencyID, out string sql_, out List<MySqlParameter> paras_);
			return await Database.ExecSqlNonQueryAsync(sql_, paras_, tm_);
		}
		private void RepairRemoveByTrdCurrencyIDData(string trdCurrencyID, out string sql_, out List<MySqlParameter> paras_)
		{
			sql_ = $"DELETE FROM {TableName} WHERE " + (trdCurrencyID != null ? "`TrdCurrencyID` = @TrdCurrencyID" : "`TrdCurrencyID` IS NULL");
			paras_ = new List<MySqlParameter>();
			if (trdCurrencyID != null)
				paras_.Add(Database.CreateInParameter("@TrdCurrencyID", trdCurrencyID, MySqlDbType.VarChar));
		}
		#endregion // RemoveByTrdCurrencyID
		#region RemoveByName
		/// <summary>
		/// 按字段删除
		/// </summary>
		/// /// <param name = "name">货币名称</param>
		/// <param name="tm_">事务管理对象</param>
		public int RemoveByName(string name, TransactionManager tm_ = null)
		{
			RepairRemoveByNameData(name, out string sql_, out List<MySqlParameter> paras_);
			return Database.ExecSqlNonQuery(sql_, paras_, tm_);
		}
		public async Task<int> RemoveByNameAsync(string name, TransactionManager tm_ = null)
		{
			RepairRemoveByNameData(name, out string sql_, out List<MySqlParameter> paras_);
			return await Database.ExecSqlNonQueryAsync(sql_, paras_, tm_);
		}
		private void RepairRemoveByNameData(string name, out string sql_, out List<MySqlParameter> paras_)
		{
			sql_ = $"DELETE FROM {TableName} WHERE " + (name != null ? "`Name` = @Name" : "`Name` IS NULL");
			paras_ = new List<MySqlParameter>();
			if (name != null)
				paras_.Add(Database.CreateInParameter("@Name", name, MySqlDbType.VarChar));
		}
		#endregion // RemoveByName
		#region RemoveByBaseUnit
		/// <summary>
		/// 按字段删除
		/// </summary>
		/// /// <param name = "baseUnit">基础单位</param>
		/// <param name="tm_">事务管理对象</param>
		public int RemoveByBaseUnit(int baseUnit, TransactionManager tm_ = null)
		{
			RepairRemoveByBaseUnitData(baseUnit, out string sql_, out List<MySqlParameter> paras_);
			return Database.ExecSqlNonQuery(sql_, paras_, tm_);
		}
		public async Task<int> RemoveByBaseUnitAsync(int baseUnit, TransactionManager tm_ = null)
		{
			RepairRemoveByBaseUnitData(baseUnit, out string sql_, out List<MySqlParameter> paras_);
			return await Database.ExecSqlNonQueryAsync(sql_, paras_, tm_);
		}
		private void RepairRemoveByBaseUnitData(int baseUnit, out string sql_, out List<MySqlParameter> paras_)
		{
			sql_ = $"DELETE FROM {TableName} WHERE `BaseUnit` = @BaseUnit";
			paras_ = new List<MySqlParameter>();
			paras_.Add(Database.CreateInParameter("@BaseUnit", baseUnit, MySqlDbType.Int32));
		}
		#endregion // RemoveByBaseUnit
		#endregion // RemoveByXXX
	    
		#region RemoveByFKOrUnique
		#endregion // RemoveByFKOrUnique
		#endregion //Remove
	    
		#region Put
		#region PutItem
		/// <summary>
		/// 更新实体到数据库
		/// </summary>
		/// <param name = "item">要更新的实体对象</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>受影响的行数</return>
		public int Put(Sp_pgsoft_currencyEO item, TransactionManager tm_ = null)
		{
			RepairPutData(item, out string sql_, out List<MySqlParameter> paras_);
			return Database.ExecSqlNonQuery(sql_, paras_, tm_);
		}
		public async Task<int> PutAsync(Sp_pgsoft_currencyEO item, TransactionManager tm_ = null)
		{
			RepairPutData(item, out string sql_, out List<MySqlParameter> paras_);
			return await Database.ExecSqlNonQueryAsync(sql_, paras_, tm_);
		}
		private void RepairPutData(Sp_pgsoft_currencyEO item, out string sql_, out List<MySqlParameter> paras_)
		{
			sql_ = $"UPDATE {TableName} SET `CurrencyID` = @CurrencyID, `TrdCurrencyID` = @TrdCurrencyID, `Name` = @Name, `BaseUnit` = @BaseUnit WHERE `CurrencyID` = @CurrencyID_Original";
			paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@CurrencyID", item.CurrencyID, MySqlDbType.VarChar),
				Database.CreateInParameter("@TrdCurrencyID", item.TrdCurrencyID != null ? item.TrdCurrencyID : (object)DBNull.Value, MySqlDbType.VarChar),
				Database.CreateInParameter("@Name", item.Name != null ? item.Name : (object)DBNull.Value, MySqlDbType.VarChar),
				Database.CreateInParameter("@BaseUnit", item.BaseUnit, MySqlDbType.Int32),
				Database.CreateInParameter("@CurrencyID_Original", item.HasOriginal ? item.OriginalCurrencyID : item.CurrencyID, MySqlDbType.VarChar),
			};
		}
		
		/// <summary>
		/// 更新实体集合到数据库
		/// </summary>
		/// <param name = "items">要更新的实体对象集合</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>受影响的行数</return>
		public int Put(IEnumerable<Sp_pgsoft_currencyEO> items, TransactionManager tm_ = null)
		{
			int ret = 0;
			foreach (var item in items)
			{
		        ret += Put(item, tm_);
			}
			return ret;
		}
		public async Task<int> PutAsync(IEnumerable<Sp_pgsoft_currencyEO> items, TransactionManager tm_ = null)
		{
			int ret = 0;
			foreach (var item in items)
			{
		        ret += await PutAsync(item, tm_);
			}
			return ret;
		}
		#endregion // PutItem
		
		#region PutByPK
		/// <summary>
		/// 按主键更新指定列数据
		/// </summary>
		/// /// <param name = "currencyID">货币类型</param>
		/// <param name = "set_">更新的列数据</param>
		/// <param name="values_">参数值</param>
		/// <return>受影响的行数</return>
		public int PutByPK(string currencyID, string set_, params object[] values_)
		{
			return Put(set_, "`CurrencyID` = @CurrencyID", ConcatValues(values_, currencyID));
		}
		public async Task<int> PutByPKAsync(string currencyID, string set_, params object[] values_)
		{
			return await PutAsync(set_, "`CurrencyID` = @CurrencyID", ConcatValues(values_, currencyID));
		}
		/// <summary>
		/// 按主键更新指定列数据
		/// </summary>
		/// /// <param name = "currencyID">货币类型</param>
		/// <param name = "set_">更新的列数据</param>
		/// <param name="tm_">事务管理对象</param>
		/// <param name="values_">参数值</param>
		/// <return>受影响的行数</return>
		public int PutByPK(string currencyID, string set_, TransactionManager tm_, params object[] values_)
		{
			return Put(set_, "`CurrencyID` = @CurrencyID", tm_, ConcatValues(values_, currencyID));
		}
		public async Task<int> PutByPKAsync(string currencyID, string set_, TransactionManager tm_, params object[] values_)
		{
			return await PutAsync(set_, "`CurrencyID` = @CurrencyID", tm_, ConcatValues(values_, currencyID));
		}
		/// <summary>
		/// 按主键更新指定列数据
		/// </summary>
		/// /// <param name = "currencyID">货币类型</param>
		/// <param name = "set_">更新的列数据</param>
		/// <param name="paras_">参数值</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>受影响的行数</return>
		public int PutByPK(string currencyID, string set_, IEnumerable<MySqlParameter> paras_, TransactionManager tm_ = null)
		{
			var newParas_ = new List<MySqlParameter>() {
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
	        };
			return Put(set_, "`CurrencyID` = @CurrencyID", ConcatParameters(paras_, newParas_), tm_);
		}
		public async Task<int> PutByPKAsync(string currencyID, string set_, IEnumerable<MySqlParameter> paras_, TransactionManager tm_ = null)
		{
			var newParas_ = new List<MySqlParameter>() {
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
	        };
			return await PutAsync(set_, "`CurrencyID` = @CurrencyID", ConcatParameters(paras_, newParas_), tm_);
		}
		#endregion // PutByPK
		
		#region PutXXX
		#region PutTrdCurrencyID
		/// <summary>
		/// 按主键更新列数据
		/// </summary>
		/// /// <param name = "currencyID">货币类型</param>
		/// /// <param name = "trdCurrencyID">对方currencyId</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>受影响的行数</return>
		public int PutTrdCurrencyIDByPK(string currencyID, string trdCurrencyID, TransactionManager tm_ = null)
		{
			RepairPutTrdCurrencyIDByPKData(currencyID, trdCurrencyID, out string sql_, out List<MySqlParameter> paras_);
			return Database.ExecSqlNonQuery(sql_, paras_, tm_);
		}
		public async Task<int> PutTrdCurrencyIDByPKAsync(string currencyID, string trdCurrencyID, TransactionManager tm_ = null)
		{
			RepairPutTrdCurrencyIDByPKData(currencyID, trdCurrencyID, out string sql_, out List<MySqlParameter> paras_);
			return await Database.ExecSqlNonQueryAsync(sql_, paras_, tm_);
		}
		private void RepairPutTrdCurrencyIDByPKData(string currencyID, string trdCurrencyID, out string sql_, out List<MySqlParameter> paras_)
		{
			sql_ = $"UPDATE {TableName} SET `TrdCurrencyID` = @TrdCurrencyID  WHERE `CurrencyID` = @CurrencyID";
			paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@TrdCurrencyID", trdCurrencyID != null ? trdCurrencyID : (object)DBNull.Value, MySqlDbType.VarChar),
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
			};
		}
	 
		/// <summary>
		/// 更新一列数据
		/// </summary>
		/// /// <param name = "trdCurrencyID">对方currencyId</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>受影响的行数</return>
		public int PutTrdCurrencyID(string trdCurrencyID, TransactionManager tm_ = null)
		{
			string sql_ = $"UPDATE {TableName} SET `TrdCurrencyID` = @TrdCurrencyID";
			var parameter_ = Database.CreateInParameter("@TrdCurrencyID", trdCurrencyID != null ? trdCurrencyID : (object)DBNull.Value, MySqlDbType.VarChar);
			return Database.ExecSqlNonQuery(sql_, new MySqlParameter[] { parameter_ }, tm_);
		}
		public async Task<int> PutTrdCurrencyIDAsync(string trdCurrencyID, TransactionManager tm_ = null)
		{
			string sql_ = $"UPDATE {TableName} SET `TrdCurrencyID` = @TrdCurrencyID";
			var parameter_ = Database.CreateInParameter("@TrdCurrencyID", trdCurrencyID != null ? trdCurrencyID : (object)DBNull.Value, MySqlDbType.VarChar);
			return await Database.ExecSqlNonQueryAsync(sql_, new MySqlParameter[] { parameter_ }, tm_);
		}
		#endregion // PutTrdCurrencyID
		#region PutName
		/// <summary>
		/// 按主键更新列数据
		/// </summary>
		/// /// <param name = "currencyID">货币类型</param>
		/// /// <param name = "name">货币名称</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>受影响的行数</return>
		public int PutNameByPK(string currencyID, string name, TransactionManager tm_ = null)
		{
			RepairPutNameByPKData(currencyID, name, out string sql_, out List<MySqlParameter> paras_);
			return Database.ExecSqlNonQuery(sql_, paras_, tm_);
		}
		public async Task<int> PutNameByPKAsync(string currencyID, string name, TransactionManager tm_ = null)
		{
			RepairPutNameByPKData(currencyID, name, out string sql_, out List<MySqlParameter> paras_);
			return await Database.ExecSqlNonQueryAsync(sql_, paras_, tm_);
		}
		private void RepairPutNameByPKData(string currencyID, string name, out string sql_, out List<MySqlParameter> paras_)
		{
			sql_ = $"UPDATE {TableName} SET `Name` = @Name  WHERE `CurrencyID` = @CurrencyID";
			paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@Name", name != null ? name : (object)DBNull.Value, MySqlDbType.VarChar),
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
			};
		}
	 
		/// <summary>
		/// 更新一列数据
		/// </summary>
		/// /// <param name = "name">货币名称</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>受影响的行数</return>
		public int PutName(string name, TransactionManager tm_ = null)
		{
			string sql_ = $"UPDATE {TableName} SET `Name` = @Name";
			var parameter_ = Database.CreateInParameter("@Name", name != null ? name : (object)DBNull.Value, MySqlDbType.VarChar);
			return Database.ExecSqlNonQuery(sql_, new MySqlParameter[] { parameter_ }, tm_);
		}
		public async Task<int> PutNameAsync(string name, TransactionManager tm_ = null)
		{
			string sql_ = $"UPDATE {TableName} SET `Name` = @Name";
			var parameter_ = Database.CreateInParameter("@Name", name != null ? name : (object)DBNull.Value, MySqlDbType.VarChar);
			return await Database.ExecSqlNonQueryAsync(sql_, new MySqlParameter[] { parameter_ }, tm_);
		}
		#endregion // PutName
		#region PutBaseUnit
		/// <summary>
		/// 按主键更新列数据
		/// </summary>
		/// /// <param name = "currencyID">货币类型</param>
		/// /// <param name = "baseUnit">基础单位</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>受影响的行数</return>
		public int PutBaseUnitByPK(string currencyID, int baseUnit, TransactionManager tm_ = null)
		{
			RepairPutBaseUnitByPKData(currencyID, baseUnit, out string sql_, out List<MySqlParameter> paras_);
			return Database.ExecSqlNonQuery(sql_, paras_, tm_);
		}
		public async Task<int> PutBaseUnitByPKAsync(string currencyID, int baseUnit, TransactionManager tm_ = null)
		{
			RepairPutBaseUnitByPKData(currencyID, baseUnit, out string sql_, out List<MySqlParameter> paras_);
			return await Database.ExecSqlNonQueryAsync(sql_, paras_, tm_);
		}
		private void RepairPutBaseUnitByPKData(string currencyID, int baseUnit, out string sql_, out List<MySqlParameter> paras_)
		{
			sql_ = $"UPDATE {TableName} SET `BaseUnit` = @BaseUnit  WHERE `CurrencyID` = @CurrencyID";
			paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@BaseUnit", baseUnit, MySqlDbType.Int32),
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
			};
		}
	 
		/// <summary>
		/// 更新一列数据
		/// </summary>
		/// /// <param name = "baseUnit">基础单位</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>受影响的行数</return>
		public int PutBaseUnit(int baseUnit, TransactionManager tm_ = null)
		{
			string sql_ = $"UPDATE {TableName} SET `BaseUnit` = @BaseUnit";
			var parameter_ = Database.CreateInParameter("@BaseUnit", baseUnit, MySqlDbType.Int32);
			return Database.ExecSqlNonQuery(sql_, new MySqlParameter[] { parameter_ }, tm_);
		}
		public async Task<int> PutBaseUnitAsync(int baseUnit, TransactionManager tm_ = null)
		{
			string sql_ = $"UPDATE {TableName} SET `BaseUnit` = @BaseUnit";
			var parameter_ = Database.CreateInParameter("@BaseUnit", baseUnit, MySqlDbType.Int32);
			return await Database.ExecSqlNonQueryAsync(sql_, new MySqlParameter[] { parameter_ }, tm_);
		}
		#endregion // PutBaseUnit
		#endregion // PutXXX
		#endregion // Put
	   
		#region Set
		
		/// <summary>
		/// 插入或者更新数据
		/// </summary>
		/// <param name = "item">要更新的实体对象</param>
		/// <param name="tm">事务管理对象</param>
		/// <return>true:插入操作；false:更新操作</return>
		public bool Set(Sp_pgsoft_currencyEO item, TransactionManager tm = null)
		{
			bool ret = true;
			if(GetByPK(item.CurrencyID) == null)
			{
				Add(item, tm);
			}
			else
			{
				Put(item, tm);
				ret = false;
			}
			return ret;
		}
		public async Task<bool> SetAsync(Sp_pgsoft_currencyEO item, TransactionManager tm = null)
		{
			bool ret = true;
			if(GetByPK(item.CurrencyID) == null)
			{
				await AddAsync(item, tm);
			}
			else
			{
				await PutAsync(item, tm);
				ret = false;
			}
			return ret;
		}
		
		#endregion // Set
		
		#region Get
		#region GetByPK
		/// <summary>
		/// 按 PK（主键） 查询
		/// </summary>
		/// /// <param name = "currencyID">货币类型</param>
		/// <param name="tm_">事务管理对象</param>
		/// <param name="isForUpdate_">是否使用FOR UPDATE锁行</param>
		/// <return></return>
		public Sp_pgsoft_currencyEO GetByPK(string currencyID, TransactionManager tm_ = null, bool isForUpdate_ = false)
		{
			RepairGetByPKData(currencyID, out string sql_, out List<MySqlParameter> paras_, isForUpdate_);
			return Database.ExecSqlSingle(sql_, paras_, tm_, Sp_pgsoft_currencyEO.MapDataReader);
		}
		public async Task<Sp_pgsoft_currencyEO> GetByPKAsync(string currencyID, TransactionManager tm_ = null, bool isForUpdate_ = false)
		{
			RepairGetByPKData(currencyID, out string sql_, out List<MySqlParameter> paras_, isForUpdate_);
			return await Database.ExecSqlSingleAsync(sql_, paras_, tm_, Sp_pgsoft_currencyEO.MapDataReader);
		}
		private void RepairGetByPKData(string currencyID, out string sql_, out List<MySqlParameter> paras_, bool isForUpdate_ = false)
		{
			sql_ = BuildSelectSQL("`CurrencyID` = @CurrencyID", 0, null, null, isForUpdate_);
			paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
			};
		}
		#endregion // GetByPK
		
		#region GetXXXByPK
		
		/// <summary>
		/// 按主键查询 TrdCurrencyID（字段）
		/// </summary>
		/// /// <param name = "currencyID">货币类型</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return></return>
		public string GetTrdCurrencyIDByPK(string currencyID, TransactionManager tm_ = null)
		{
			var paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
			};
			return (string)GetScalar("`TrdCurrencyID`", "`CurrencyID` = @CurrencyID", paras_, tm_);
		}
		public async Task<string> GetTrdCurrencyIDByPKAsync(string currencyID, TransactionManager tm_ = null)
		{
			var paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
			};
			return (string)await GetScalarAsync("`TrdCurrencyID`", "`CurrencyID` = @CurrencyID", paras_, tm_);
		}
		
		/// <summary>
		/// 按主键查询 Name（字段）
		/// </summary>
		/// /// <param name = "currencyID">货币类型</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return></return>
		public string GetNameByPK(string currencyID, TransactionManager tm_ = null)
		{
			var paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
			};
			return (string)GetScalar("`Name`", "`CurrencyID` = @CurrencyID", paras_, tm_);
		}
		public async Task<string> GetNameByPKAsync(string currencyID, TransactionManager tm_ = null)
		{
			var paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
			};
			return (string)await GetScalarAsync("`Name`", "`CurrencyID` = @CurrencyID", paras_, tm_);
		}
		
		/// <summary>
		/// 按主键查询 BaseUnit（字段）
		/// </summary>
		/// /// <param name = "currencyID">货币类型</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return></return>
		public int GetBaseUnitByPK(string currencyID, TransactionManager tm_ = null)
		{
			var paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
			};
			return (int)GetScalar("`BaseUnit`", "`CurrencyID` = @CurrencyID", paras_, tm_);
		}
		public async Task<int> GetBaseUnitByPKAsync(string currencyID, TransactionManager tm_ = null)
		{
			var paras_ = new List<MySqlParameter>() 
			{
				Database.CreateInParameter("@CurrencyID", currencyID, MySqlDbType.VarChar),
			};
			return (int)await GetScalarAsync("`BaseUnit`", "`CurrencyID` = @CurrencyID", paras_, tm_);
		}
		#endregion // GetXXXByPK
		#region GetByXXX
		#region GetByTrdCurrencyID
		
		/// <summary>
		/// 按 TrdCurrencyID（字段） 查询
		/// </summary>
		/// /// <param name = "trdCurrencyID">对方currencyId</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByTrdCurrencyID(string trdCurrencyID)
		{
			return GetByTrdCurrencyID(trdCurrencyID, 0, string.Empty, null);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByTrdCurrencyIDAsync(string trdCurrencyID)
		{
			return await GetByTrdCurrencyIDAsync(trdCurrencyID, 0, string.Empty, null);
		}
		
		/// <summary>
		/// 按 TrdCurrencyID（字段） 查询
		/// </summary>
		/// /// <param name = "trdCurrencyID">对方currencyId</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByTrdCurrencyID(string trdCurrencyID, TransactionManager tm_)
		{
			return GetByTrdCurrencyID(trdCurrencyID, 0, string.Empty, tm_);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByTrdCurrencyIDAsync(string trdCurrencyID, TransactionManager tm_)
		{
			return await GetByTrdCurrencyIDAsync(trdCurrencyID, 0, string.Empty, tm_);
		}
		
		/// <summary>
		/// 按 TrdCurrencyID（字段） 查询
		/// </summary>
		/// /// <param name = "trdCurrencyID">对方currencyId</param>
		/// <param name = "top_">获取行数</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByTrdCurrencyID(string trdCurrencyID, int top_)
		{
			return GetByTrdCurrencyID(trdCurrencyID, top_, string.Empty, null);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByTrdCurrencyIDAsync(string trdCurrencyID, int top_)
		{
			return await GetByTrdCurrencyIDAsync(trdCurrencyID, top_, string.Empty, null);
		}
		
		/// <summary>
		/// 按 TrdCurrencyID（字段） 查询
		/// </summary>
		/// /// <param name = "trdCurrencyID">对方currencyId</param>
		/// <param name = "top_">获取行数</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByTrdCurrencyID(string trdCurrencyID, int top_, TransactionManager tm_)
		{
			return GetByTrdCurrencyID(trdCurrencyID, top_, string.Empty, tm_);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByTrdCurrencyIDAsync(string trdCurrencyID, int top_, TransactionManager tm_)
		{
			return await GetByTrdCurrencyIDAsync(trdCurrencyID, top_, string.Empty, tm_);
		}
		
		/// <summary>
		/// 按 TrdCurrencyID（字段） 查询
		/// </summary>
		/// /// <param name = "trdCurrencyID">对方currencyId</param>
		/// <param name = "sort_">排序表达式</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByTrdCurrencyID(string trdCurrencyID, string sort_)
		{
			return GetByTrdCurrencyID(trdCurrencyID, 0, sort_, null);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByTrdCurrencyIDAsync(string trdCurrencyID, string sort_)
		{
			return await GetByTrdCurrencyIDAsync(trdCurrencyID, 0, sort_, null);
		}
		
		/// <summary>
		/// 按 TrdCurrencyID（字段） 查询
		/// </summary>
		/// /// <param name = "trdCurrencyID">对方currencyId</param>
		/// <param name = "sort_">排序表达式</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByTrdCurrencyID(string trdCurrencyID, string sort_, TransactionManager tm_)
		{
			return GetByTrdCurrencyID(trdCurrencyID, 0, sort_, tm_);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByTrdCurrencyIDAsync(string trdCurrencyID, string sort_, TransactionManager tm_)
		{
			return await GetByTrdCurrencyIDAsync(trdCurrencyID, 0, sort_, tm_);
		}
		
		/// <summary>
		/// 按 TrdCurrencyID（字段） 查询
		/// </summary>
		/// /// <param name = "trdCurrencyID">对方currencyId</param>
		/// <param name = "top_">获取行数</param>
		/// <param name = "sort_">排序表达式</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByTrdCurrencyID(string trdCurrencyID, int top_, string sort_, TransactionManager tm_)
		{
			var sql_ = BuildSelectSQL(trdCurrencyID != null ? "`TrdCurrencyID` = @TrdCurrencyID" : "`TrdCurrencyID` IS NULL", top_, sort_);
			var paras_ = new List<MySqlParameter>();
			if (trdCurrencyID != null)
				paras_.Add(Database.CreateInParameter("@TrdCurrencyID", trdCurrencyID, MySqlDbType.VarChar));
			return Database.ExecSqlList(sql_, paras_, tm_, Sp_pgsoft_currencyEO.MapDataReader);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByTrdCurrencyIDAsync(string trdCurrencyID, int top_, string sort_, TransactionManager tm_)
		{
			var sql_ = BuildSelectSQL(trdCurrencyID != null ? "`TrdCurrencyID` = @TrdCurrencyID" : "`TrdCurrencyID` IS NULL", top_, sort_);
			var paras_ = new List<MySqlParameter>();
			if (trdCurrencyID != null)
				paras_.Add(Database.CreateInParameter("@TrdCurrencyID", trdCurrencyID, MySqlDbType.VarChar));
			return await Database.ExecSqlListAsync(sql_, paras_, tm_, Sp_pgsoft_currencyEO.MapDataReader);
		}
		#endregion // GetByTrdCurrencyID
		#region GetByName
		
		/// <summary>
		/// 按 Name（字段） 查询
		/// </summary>
		/// /// <param name = "name">货币名称</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByName(string name)
		{
			return GetByName(name, 0, string.Empty, null);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByNameAsync(string name)
		{
			return await GetByNameAsync(name, 0, string.Empty, null);
		}
		
		/// <summary>
		/// 按 Name（字段） 查询
		/// </summary>
		/// /// <param name = "name">货币名称</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByName(string name, TransactionManager tm_)
		{
			return GetByName(name, 0, string.Empty, tm_);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByNameAsync(string name, TransactionManager tm_)
		{
			return await GetByNameAsync(name, 0, string.Empty, tm_);
		}
		
		/// <summary>
		/// 按 Name（字段） 查询
		/// </summary>
		/// /// <param name = "name">货币名称</param>
		/// <param name = "top_">获取行数</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByName(string name, int top_)
		{
			return GetByName(name, top_, string.Empty, null);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByNameAsync(string name, int top_)
		{
			return await GetByNameAsync(name, top_, string.Empty, null);
		}
		
		/// <summary>
		/// 按 Name（字段） 查询
		/// </summary>
		/// /// <param name = "name">货币名称</param>
		/// <param name = "top_">获取行数</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByName(string name, int top_, TransactionManager tm_)
		{
			return GetByName(name, top_, string.Empty, tm_);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByNameAsync(string name, int top_, TransactionManager tm_)
		{
			return await GetByNameAsync(name, top_, string.Empty, tm_);
		}
		
		/// <summary>
		/// 按 Name（字段） 查询
		/// </summary>
		/// /// <param name = "name">货币名称</param>
		/// <param name = "sort_">排序表达式</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByName(string name, string sort_)
		{
			return GetByName(name, 0, sort_, null);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByNameAsync(string name, string sort_)
		{
			return await GetByNameAsync(name, 0, sort_, null);
		}
		
		/// <summary>
		/// 按 Name（字段） 查询
		/// </summary>
		/// /// <param name = "name">货币名称</param>
		/// <param name = "sort_">排序表达式</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByName(string name, string sort_, TransactionManager tm_)
		{
			return GetByName(name, 0, sort_, tm_);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByNameAsync(string name, string sort_, TransactionManager tm_)
		{
			return await GetByNameAsync(name, 0, sort_, tm_);
		}
		
		/// <summary>
		/// 按 Name（字段） 查询
		/// </summary>
		/// /// <param name = "name">货币名称</param>
		/// <param name = "top_">获取行数</param>
		/// <param name = "sort_">排序表达式</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByName(string name, int top_, string sort_, TransactionManager tm_)
		{
			var sql_ = BuildSelectSQL(name != null ? "`Name` = @Name" : "`Name` IS NULL", top_, sort_);
			var paras_ = new List<MySqlParameter>();
			if (name != null)
				paras_.Add(Database.CreateInParameter("@Name", name, MySqlDbType.VarChar));
			return Database.ExecSqlList(sql_, paras_, tm_, Sp_pgsoft_currencyEO.MapDataReader);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByNameAsync(string name, int top_, string sort_, TransactionManager tm_)
		{
			var sql_ = BuildSelectSQL(name != null ? "`Name` = @Name" : "`Name` IS NULL", top_, sort_);
			var paras_ = new List<MySqlParameter>();
			if (name != null)
				paras_.Add(Database.CreateInParameter("@Name", name, MySqlDbType.VarChar));
			return await Database.ExecSqlListAsync(sql_, paras_, tm_, Sp_pgsoft_currencyEO.MapDataReader);
		}
		#endregion // GetByName
		#region GetByBaseUnit
		
		/// <summary>
		/// 按 BaseUnit（字段） 查询
		/// </summary>
		/// /// <param name = "baseUnit">基础单位</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByBaseUnit(int baseUnit)
		{
			return GetByBaseUnit(baseUnit, 0, string.Empty, null);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByBaseUnitAsync(int baseUnit)
		{
			return await GetByBaseUnitAsync(baseUnit, 0, string.Empty, null);
		}
		
		/// <summary>
		/// 按 BaseUnit（字段） 查询
		/// </summary>
		/// /// <param name = "baseUnit">基础单位</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByBaseUnit(int baseUnit, TransactionManager tm_)
		{
			return GetByBaseUnit(baseUnit, 0, string.Empty, tm_);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByBaseUnitAsync(int baseUnit, TransactionManager tm_)
		{
			return await GetByBaseUnitAsync(baseUnit, 0, string.Empty, tm_);
		}
		
		/// <summary>
		/// 按 BaseUnit（字段） 查询
		/// </summary>
		/// /// <param name = "baseUnit">基础单位</param>
		/// <param name = "top_">获取行数</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByBaseUnit(int baseUnit, int top_)
		{
			return GetByBaseUnit(baseUnit, top_, string.Empty, null);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByBaseUnitAsync(int baseUnit, int top_)
		{
			return await GetByBaseUnitAsync(baseUnit, top_, string.Empty, null);
		}
		
		/// <summary>
		/// 按 BaseUnit（字段） 查询
		/// </summary>
		/// /// <param name = "baseUnit">基础单位</param>
		/// <param name = "top_">获取行数</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByBaseUnit(int baseUnit, int top_, TransactionManager tm_)
		{
			return GetByBaseUnit(baseUnit, top_, string.Empty, tm_);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByBaseUnitAsync(int baseUnit, int top_, TransactionManager tm_)
		{
			return await GetByBaseUnitAsync(baseUnit, top_, string.Empty, tm_);
		}
		
		/// <summary>
		/// 按 BaseUnit（字段） 查询
		/// </summary>
		/// /// <param name = "baseUnit">基础单位</param>
		/// <param name = "sort_">排序表达式</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByBaseUnit(int baseUnit, string sort_)
		{
			return GetByBaseUnit(baseUnit, 0, sort_, null);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByBaseUnitAsync(int baseUnit, string sort_)
		{
			return await GetByBaseUnitAsync(baseUnit, 0, sort_, null);
		}
		
		/// <summary>
		/// 按 BaseUnit（字段） 查询
		/// </summary>
		/// /// <param name = "baseUnit">基础单位</param>
		/// <param name = "sort_">排序表达式</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByBaseUnit(int baseUnit, string sort_, TransactionManager tm_)
		{
			return GetByBaseUnit(baseUnit, 0, sort_, tm_);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByBaseUnitAsync(int baseUnit, string sort_, TransactionManager tm_)
		{
			return await GetByBaseUnitAsync(baseUnit, 0, sort_, tm_);
		}
		
		/// <summary>
		/// 按 BaseUnit（字段） 查询
		/// </summary>
		/// /// <param name = "baseUnit">基础单位</param>
		/// <param name = "top_">获取行数</param>
		/// <param name = "sort_">排序表达式</param>
		/// <param name="tm_">事务管理对象</param>
		/// <return>实体对象集合</return>
		public List<Sp_pgsoft_currencyEO> GetByBaseUnit(int baseUnit, int top_, string sort_, TransactionManager tm_)
		{
			var sql_ = BuildSelectSQL("`BaseUnit` = @BaseUnit", top_, sort_);
			var paras_ = new List<MySqlParameter>();
			paras_.Add(Database.CreateInParameter("@BaseUnit", baseUnit, MySqlDbType.Int32));
			return Database.ExecSqlList(sql_, paras_, tm_, Sp_pgsoft_currencyEO.MapDataReader);
		}
		public async Task<List<Sp_pgsoft_currencyEO>> GetByBaseUnitAsync(int baseUnit, int top_, string sort_, TransactionManager tm_)
		{
			var sql_ = BuildSelectSQL("`BaseUnit` = @BaseUnit", top_, sort_);
			var paras_ = new List<MySqlParameter>();
			paras_.Add(Database.CreateInParameter("@BaseUnit", baseUnit, MySqlDbType.Int32));
			return await Database.ExecSqlListAsync(sql_, paras_, tm_, Sp_pgsoft_currencyEO.MapDataReader);
		}
		#endregion // GetByBaseUnit
		#endregion // GetByXXX
		#endregion // Get
	}
	#endregion // MO
}
