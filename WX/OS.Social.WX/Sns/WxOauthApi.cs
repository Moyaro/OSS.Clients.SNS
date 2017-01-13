﻿#region Copyright (C) 2016 OS系列开源项目

/***************************************************************************
*　　	文件功能描述：Oauth2.0  授权基类
*
*　　	创建人： 王超
*       创建人Email：1985088337@qq.com
*    	创建日期：2016
*       
*****************************************************************************/

#endregion


using OS.Http;
using OS.Http.Models;
using OS.Social.WX.Sns.Mos;

namespace OS.Social.WX.Sns
{
    public class WxOauthApi:WxBaseApi
    {
        public WxOauthApi(WxAppCoinfig config) : base(config)
        {
        }

        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="redirectUri">授权后重定向的回调链接地址，请使用urlencode对链接进行处理</param>
        /// <param name="type">授权客户端类型，如果是pc，则生成的是微信页面二维码授权页</param>
        /// <returns></returns>
        public string GetAuthorizeUrl(string redirectUri, AuthClientType type)
        {
            if (type == AuthClientType.WxOffcial)
            {
                return
                    $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={m_Config.AppId}&redirect_uri={redirectUri}&response_type=code&scope=snsapi_userinfo&state={m_Config.AppSource}#wechat_redirect";
            }
            return
                $"https://open.weixin.qq.com/connect/qrconnect?appid={m_Config.AppId}&redirect_uri={redirectUri}&response_type=code&scope=snsapi_login&state={m_Config.AppSource}#wechat_redirect";
        }

        /// <summary>
        /// 获取授权access_token   (每个用户都是单独唯一)
        /// </summary>
        /// <param name="code">填写第一步获取的code参数</param>
        /// <returns></returns>
        public WxGetAccessTokenResp GetAuthAccessToken(string code)
        {
            var req=new OsHttpRequest();

            req.AddressUrl = $"{m_ApiUrl}/sns/oauth2/access_token?appid={m_Config.AppId}&secret={m_Config.AppSecret}&code={code}&grant_type=authorization_code";
            req.HttpMothed=HttpMothed.GET;

            return RestCommon<WxGetAccessTokenResp>(req);
        }

        /// <summary>
        ///   刷新当前用户授权Token
        /// </summary>
        /// <param name="accessToken">授权接口调用凭证</param>
        /// <returns></returns>
        public WxGetAccessTokenResp RefreshAuthAccessToken(string accessToken)
        {
            var request = new OsHttpRequest();

            request.AddressUrl = $"{m_ApiUrl}/sns/oauth2/refresh_token?appid={m_Config.AppId}&grant_type=refresh_token&refresh_token={accessToken}";
            request.HttpMothed = HttpMothed.GET;

            return RestCommon<WxGetAccessTokenResp>(request);
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessToken">授权接口调用凭证</param>
        /// <param name="openId">用户的唯一标识</param>
        /// <returns></returns>
        public WxGetAuthUserResp GetWxAuthUserInfo(string accessToken, string openId)
        {
            var request = new OsHttpRequest();
            request.AddressUrl = $"{m_ApiUrl}/sns/userinfo?access_token={accessToken}&openid={openId}";
            request.HttpMothed = HttpMothed.GET;

            return RestCommon<WxGetAuthUserResp>(request);
        }



        /// <summary>
        /// 检验授权凭证（access_token）是否有效
        /// </summary>
        /// <param name="accessToken">授权接口调用凭证</param>
        /// <param name="openId">用户的唯一标识</param>
        /// <returns></returns>
        public WxBaseResp CheckAccessToken(string accessToken, string openId)
        {
            string url = $"{m_ApiUrl}/sns/auth?access_token={accessToken}&openid={openId}";

            var request = new OsHttpRequest();
            request.AddressUrl = url;
            request.HttpMothed = HttpMothed.GET;

            return RestCommon<WxBaseResp>(request);
        }

    }
}
