using EasyNetQ.ConnectionString;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AiUo;
using AiUo.Data;
using AiUo.Logging;
using UGame.Bridge.Service.Operator;
using UGame.Bridge.Service.Provider;
using Xxyy.Common.Caching;
using Xxyy.DAL;
using UGame.Bridge.Model.Common;

namespace UGame.Bridge.Service.Common
{
    public static class PartnerUtil
    {
        #region ProviderProxy
        // key: providerId
        private static ConcurrentDictionary<string, BaseProviderProxy> _providerDic = new();
        public static BaseProviderProxy CreateProviderProxy(string providerId)
        {
            if (!_providerDic.TryGetValue(providerId, out var ret))
            {
                var prov = DbCacheUtil.GetProvider(providerId);
                switch (prov.ProviderType)
                {
                    case 0: // 自有的
                        ret = new MyProviderProxy(providerId);
                        break;
                    case 1: // 第三方接我方的标准接口
                        ret = new ThirdProviderProxy(providerId);
                        break;
                    case 2: // 我方接第三方提供的接口
                        var logger = LogUtil.GetContextLogger()
                            .AddMessage("执行PartnerUtil.CreateProviderProxy")
                            .AddField("PartnerUtil.providerId", providerId);
                        // _分割的providerId使用前缀相同的DLL
                        var providerDllName = providerId.Split('_')[0].PascalCase();
                        var name = $"Xxyy.Providers.{providerDllName}";
                        var asmFile = Path.Combine(AppContext.BaseDirectory, $"{name}.dll");
                        //var asmFile = $"{name}.dll";
                        logger.AddField("PartnerUtil.asmFile", asmFile);
                        if (!File.Exists(asmFile))
                            throw new Exception($"Xxyy.Providers.xxx DLL不存在。dll: {asmFile}");
                        var typeName = $"{name}.ProviderProxy";
                        var asm = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, asmFile));
                        var type = asm.GetType(typeName, true, true);
                        ret = Activator.CreateInstance(type, providerId) as BaseProviderProxy;
                        if (ret == null)
                            throw new Exception($"{typeName}不存在。dll: {asmFile}");
                        break;
                }
                _providerDic.TryAdd(providerId, ret);
            }
            return ret;
        }
        #endregion

        #region OperatorProxy
        private static ConcurrentDictionary<string, BaseOperatorProxy> _operatorDic = new();
        public static BaseOperatorProxy CreateOperatorProxy(string operatorId)
        {
            if (!_operatorDic.TryGetValue(operatorId, out var ret))
            {
                var oper = DbCacheUtil.GetOperator(operatorId);
                switch (oper.OperatorType)
                {
                    case 0:
                        ret = new MyOperatorProxy(operatorId);
                        break;
                    case 1:
                        ret = new ThirdOperatorProxy(operatorId);
                        break;
                    case 2:
                        var name = $"Xxyy.Operators.{operatorId.PascalCase()}";
                        var asmFile = $"{name}.dll";
                        var typeName = $"{name}.OperatorProxy";
                        var asm = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, asmFile));
                        var type = asm.GetType(typeName, true, true);
                        ret = Activator.CreateInstance(type, operatorId) as BaseOperatorProxy;
                        if (ret == null)
                            throw new Exception($"{typeName}不存在。dll: {asmFile}");
                        break;
                }
                // 自有
                _operatorDic.TryAdd(operatorId, ret);
            }
            return ret;
        }
        #endregion

        #region Methods
        public static void ThrowIfNull(string value, string msg)
        {
            if (string.IsNullOrEmpty(value))
                throw new CustomException(ResponseCodes.RS_WRONG_SYNTAX, msg);
        }
        public static void ThrowIfFunc(Func<bool> func, string msg, string resCodes = ResponseCodes.RS_WRONG_SYNTAX)
        {
            if (func())
                throw new CustomException(resCodes, msg);
        }

        public static void CheckPartnerMoney(decimal money, string currencyId)
        {
            var baseUnit = DbCacheUtil.GetCurrency(currencyId).BaseUnit;
            var maxValue = 100000000;//输赢1万
            if (money > maxValue / baseUnit)
                throw new CustomException(ResponseCodes.RS_NOT_ENOUGH_MONEY, $"Partner传入的金额超过限制: maxValue:{maxValue}");
        }

        private static S_provider_trans_logMO _provLogMo = new();
        private static HashSet<string> _logAppIds = new()
        {
        };
        public static async Task AddProviderTransLog(S_provider_trans_logEO logEo, TransactionManager tm, string operatorId, string appId)
        {
            if (_logAppIds.Count == 0 || !_logAppIds.Contains(appId))
                return;
            await _provLogMo.AddAsync(logEo, tm);
        }

        public static void ClearCaching()
        {
            _providerDic.Clear();
            _operatorDic.Clear();
        }
        #endregion
    }
}
