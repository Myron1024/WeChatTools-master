# 防封自动切换系统部署

> 1.下载此文件所在的文件夹publish，windows部署iis，指向这个publish文件(www.aaa.com)，应用池选择.net 4.0

> 2.打开config/HostUrl.config, 配置域名池，下面这些域名泛解析指向用户最终要访问的站点（也就是大家要推广的站点）
```
<add key="Host" value="www.bbb.com,11.ccc.com,222.ddd.com,www.ddd.com" />
```
  
> 3.打开config/WeiXin.config, 配置微信域名检测的api和key，默认采用测试的api和key.
```
  <add key="wxCheckApi" value="https://api.kuweimi.com/pro/wxUrlCheck.ashx" />
  <add key="wxCheckApiKey" value="341e0b5df120394ec99e517b67774399" />
```

> 4.访问 http://www.aaa.com/dev/urlTransfer.ashx?url={传用户当前推广的网址}&domain={传用户当前推广的域名(url=网址里面的域名，主要是分割url用的)}
```
例如：
http://www.aaa.com/dev/urlTransfer.ashx?domain=www.bbb.com&url=http%3a%2f%2fwww.bbb.com%2findex.php%3fg%3dWap%26m%3dVote%26a%3dindex%26token%3duDSrEHNs9CFGcTSC%26wecha_id%3docMqvwRjzPH9eseHRc_Z9nlP-DSM%26id%3d25%26iMicms%3dmp.weixin.qq.com
```
- 注意：url=需要http编码过的

> 5.修改大家需要推广的站点，分享链接，推送链接等等