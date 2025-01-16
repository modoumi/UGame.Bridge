using AiUo;
using AiUo.DbCaching;
using UGame.Bridge.Client;
using UGame.Bridge.Model;

namespace UGame.Bridge.Client.Caching
{
    internal static class DbCacheUtil
    {
        #region s_app
        public static S_appPO GetApp(string appId, bool exceptionOnNull = true, string errorCode = null)
        {
            var ret = DbCachingUtil.GetSingle<S_appPO>(appId);
            if (ret == null)
            {
                if (exceptionOnNull)
                {
                    if (string.IsNullOrEmpty(errorCode))
                        throw new Exception($"AppId不存在: {appId}");
                    else
                        throw new CustomException(errorCode, $"AppId不存在: {appId}");
                }
                else
                    return null;
            }
            return ret;
        }
        #endregion

        #region s_provider
        public static S_providerPO GetProvider(string providerId, bool excOnNull = true, string errorCode = null)
        {
            var ret = DbCachingUtil.GetSingle<S_providerPO>(providerId);
            if (ret == null)
            {
                if (excOnNull)
                {
                    if (string.IsNullOrEmpty(errorCode))
                        throw new Exception($"providerId不存在: {providerId}");
                    else
                        throw new CustomException(errorCode, $"providerId不存在: {providerId}");
                }
                else
                    return null;
            }
            return ret;
        }
        public static IProviderConfig GetProviderConfig(string providerId)
        {
            var dict = DbCachingUtil.GetOrAddCustom<S_providerPO, Dictionary<string, IProviderConfig>>("UGame.Bridge.Client.Caching.ProviderConfig", (list) =>
            {
                var cacheDict = new Dictionary<string, IProviderConfig>();
                foreach (var item in list)
                {
                    var json = item.ProviderConfig?.Trim().Replace("\r", "").Replace("\n", "");
                    var config = !string.IsNullOrEmpty(json)
                        ? SerializerUtil.DeserializeJsonNet<BaseProviderConfig>(json) : null;
                    cacheDict.Add(item.ProviderID, config);
                }
                return cacheDict;
            });
            if (!dict.TryGetValue(providerId, out var ret))
                throw new Exception($"providerId不存在: {providerId}");
            return ret;
        }
        #endregion

        #region s_operator
        public static V_s_operatorPO GetOperator(string operatorId, bool excOnNull = true, string errorCode = null)
        {
            var ret = DbCachingUtil.GetSingle<V_s_operatorPO>(it => it.OperatorID, operatorId);
            if (ret == null)
            {
                if (excOnNull)
                {
                    if (string.IsNullOrEmpty(errorCode))
                        throw new Exception($"无效的operatorId: {operatorId}");
                    else
                        throw new CustomException(errorCode, $"无效的operatorId: {operatorId}");
                }
                else
                    return null;
            }
            return ret;
        }
        #endregion
    }
}
