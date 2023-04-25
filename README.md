
## 介绍

💕在学习/工作的过程中，不断遇到问题，不断积累，在此记录。如果你也遇到同样问题，可以提出，我们将共同构建它！

### 🐣开始

```shell
$ git clone https://gitee.com/xiaolipro/fake.git
$ cd fake
$ dotnet restore
```

### 😂请帮帮我
- mutil-framework compatibility
- unit test，functional test，benchmark test and more
- simple demo，useage doc
- bug issue and idel

### 🐌基础能力
- [x] 模块化
- [x] 依赖注入
- [x] 审计日志
- [x] 本地化（多语言）
- [x] 虚拟文件系统
- [x] 工作单元
- [x] 发布者模式&本地事件总线
  - [x] 事件处理器
  - [x] 事件订阅器
  - [x] 事件发布器
  - [x] 事件存储器

### 🐸微服务能力
- [ ] Consul服务发现与注册
- [ ] Grpc客户端负载均衡
- [x] RabbitMQ分布式事件总线
- [ ] SkyWalking分布式链路追踪


#### Grpc
- 可自定义客户端负载均衡算法/服务解析器
- 日志拦截器，skywalking上报
- 异常抛出GrpcException，可被管道捕获（适合host）
- 回退机制，客户端先起，阻塞等待可用服务出现（适合work）
- 服务端掉线，自动踢出可用队列
- 服务不可达/grpc内部异常，自动重试机制
- 连接持活机制，服务队列缓存机制等