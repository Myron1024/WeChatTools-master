# 微信工具集

交流QQ群：316344705

## 项目介绍
> 微信域名网址检测
- 微信域名检测("已停止访问该网页","网页包含诱导分享、诱导关注内容，被多人投诉，为维护绿色上网环境，已停止访问")
- 见pro/wxUrlCheck.ashx.cs

> 域名切换
- 微信公众号网页域名随机生成跳转，配合微信域名检测功能，自动切换域名，保证活动正常访问
- 见pro/urlTransfer.ashx.cs

> 微信公众号图片中转
- 微信公众号文章里面的图片不能嵌套其它页面进行访问，做了微信做了防盗链处理，所以我们做了一个中转。
- 见 pro/hdShowImg.ashx.cs

## 使用
- 域名检测试用接口 [https://api.kuweimi.com/pro/wxUrlCheck.ashx?url=http://www.teu7.cn](https://api.kuweimi.com/pro/wxUrlCheck.ashx?url=http://www.teu7.cn "域名检测试用接口")
```
 {"State":true, "Data":"www.teu7.cn","Msg":"屏蔽"}
 {"State":true, "Data":"jingdong.com","Msg":"正常"}
 {"State":false,"Data":"jingdong.com","Msg":"非法访问，访问被拒绝,进qq群交流:316344705"}
 {"State":false,"Data":"jingdong.com","Msg":"歇一歇,访问太快了,进qq群交流:316344705"}
 {"State":false,"Data":"jingdong.com","Msg":"服务暂停,请联系管理员!"}
```
- 域名检测界面：https://api.kuweimi.com/
 
### 顶尖技术，铸就稳定服务

微信域名检测接口采用基于系统服务开发，接口快速、稳定。

### 官方接口，实时结果

    微信域名检测采用微信官方接口，实时返回查询结果，准确率100%，API接口响应速度快，平均检测时间只需0.2秒

### 在线自助查询/API集成查询

    微信域名检测接口支持多域名不限次数提交查询，web端在线查询返回结果，无需安装插件或客户端。支持API查询、支持集成到自由系统或第三方系统

### 为用户定制的域名自动监测系统

    为了方便用户，可以为用户开发了一套域名自动检测自动切换系统，保证微信公众号活动正常进行。

> 免费提供独立的微信域名检测api, 

> 防封保护，提供没有被微信屏蔽的二级域名，指向我指定的ip,然后告诉我,你的ip和端口（这个域名指向你的站点的ip和端口）
 