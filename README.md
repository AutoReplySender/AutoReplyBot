# AutoReplyBot

用于 [BAND](https://band.us/) 的自动回复 bot。支持回复主贴、回复贴及楼中楼，对 bot 加入的各个 BAND 均有效。

## 配置文件说明

bot 使用的各项配置文件保存在 `configs` 文件夹下。

### 关键词及语录：`rules.yaml`

```yaml
# 每条语录是数组中的一个元素
  keywords: [关键词一,关键词二] # 触发bot的关键词，支持"*"通配符始终触发
  target_authors: ["*"] # 触发bot的目标用户，支持"*"通配符始终触发
  # 目标用户与关键词两个条件须同时满足才会触发bot
  replies: # 触发bot后发送的回复。支持设定多条回复，bot会随机选择一条发送
    - data: 回复文本一
      reply_type: 0 # 0代表正常文本，1代表需要求值的C#表达式
    - data: 回复文本二
      reply_type: 0
  ignore_case: false # 可选，设置为false时关键词区分大小写，默认不区分大小写
  trigger_chance: 100 # 可选，满足触发条件时触发bot的概率的百分数，支持小数，默认为100%触发
  emotion_type: sad # 可选，满足触发条件时设置的表情，默认不设置表情。当前BAND支持great, funny, like, shocked, sad, angry六种表情
  type: comment # 可选，设置为comment时仅回复可以触发bot，为post时仅主贴可以触发bot。默认主贴和回复均可触发
```

### 运行配置：`config.json`

```json
{
  "Email": "", <- bot BAND账号的邮箱
  "Password": "", <- bot BAND账号的密码
  "ChromeDriverDir": "", <- Chrome Webdriver所在目录
  "ChromePath": "/usr/bin/chrome-browser", <- Chrome浏览器所在目录
  "Consumers": 4, <- 发送回复的线程数
  "Proxy": "http://localhost:8888", <- bot使用的代理，不用代理请设置为null
  "MaxTriggerTimesBySinglePost": 9 <- 单条回复能触发的bot回复数量的上限
}
```

### 数据库初始化：`db.sql`

初始化数据库要执行的 DDL 语句。配置环境时运行即可，日常使用 bot 不会用到。

### 浏览器 cookie 保存：`saved.cookies`

bot 保存的浏览器 cookie，预先准备好此文件则无需使用邮箱、密码登录。要获取 cookie，请使用 Chrome 浏览器登录 BAND 并勾选“记住登录”，之后访问 auth.band.us，在浏览器开发者工具—控制台中输入 `document.cookie` 获得 cookie 字符串，将其保存即可。整个字符串的开头和结尾不要加引号。

## 环境配置

编译本 bot 的源代码需要安装 [dotnet SDK](https://dotnet.microsoft.com/en-us/download)，版本为7.0。可以使用 IDE （Visual Studio，Rider）或 [dotnet publish](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish) 命令进行编译。

运行本 bot 需要安装 [PostgreSQL](https://www.postgresql.org/download/) 数据库。安装完成后，请将 `pg_hba.conf` 文件中本地（local）访问的信任级别由 peer 或 md5 级别改为 trust 级别，或是修改 `Model.cs` 文件中 `AutoReplyContext` 类的数据库连接字符串，配置好用户名和密码。接下来，请运行 `db.sql` 中的 DDL 代码，创建 bot 需要的数据库表：

```shell
cd AutoReplyBot
psql -U [用户名，默认为postgres] -d [数据库名，默认为postgres] -a -f ./configs/db.sql
```

请根据**配置文件说明**一节设置好各配置文件，然后运行 bot：

```shell
chmod +x ./AutoReplyBot
./AutoReplyBot
```

如果使用 Linux 服务器，可以使用 [screen](https://tldr.ostera.io/screen) 将 bot 放置在后台运行。

## 命令行选项

```shell
AutoReplyBot: ./AutoReplyBot [--login] [--headless]
    --login    使用Selenium-Webdriver控制Chrome浏览器输入用户名和密码登录BAND。默认读取saved.cookies中的cookie登录
    --headless    将Chrome浏览器设置为无头模式。默认为图形界面模式
```
