# Zero 
自动化构建工具

## Stages

Stages 表示构建阶段，说白了就是上面提到的流程。默认有3个stages：build, test, deploy。我们可以在一次 Pipeline 中定义多个 Stages，这些 Stages 会有以下特点：

1. 所有 Stages 会按照顺序运行，即当一个 Stage 完成后，下一个 Stage 才会开始
2. 只有当所有 Stages 完成后，该构建任务 (Pipeline) 才会成功
3. 如果任何一个 Stage 失败，那么后面的 Stages 不会执行，该构建任务 (Pipeline) 失败

## Jobs

Jobs 表示构建工作，表示某个 Stage 里面执行的工作。我们可以在 Stages 里面定义多个 Jobs，这些 Jobs 会有以下特点：

1. 相同 Stage 中的 Jobs 会并行执行

2. 相同 Stage 中的 Jobs 都执行成功时，该 Stage 才会成功

3. 如果任何一个 Job 失败，那么该 Stage 失败，即该构建任务 (Pipeline) 失败