const Koa = require('koa')
const path = require('path');
const serve = require('koa-static')
const Router = require('koa-router')
const bodyParser = require('koa-bodyparser')
const mongoose = require('mongoose')

const addHistory = require('./tools/add-history')
const starTime = require('./tools/start-time')

const app = new Koa()
const router = new Router()

mongoose.connect('mongodb://localhost:27017/userRequestTimes', {useNewUrlParser: true})
.then(() => {
    console.log('Mongodb Connected...')
})
.catch(err => {
    console.log(err)
})
mongoose.set('useFindAndModify', false)

// 定时时执行 用户当日使用情况日结
starTime({
    startTime: 0, 
    func:addHistory, 
    islog: true
})


router.get('/github', async ctx => {
    console.log(ctx.headers.origin)
    ctx.set('Access-Control-Allow-Origin', ctx.headers.origin);
    ctx.body = '哈哈你拿到我了'
    console.log(ctx.query.access_token)
})

//https://github.com/login/oauth/authorize?client_id=2bad7ade7264aaafee4b&scope=user:email    
//http://47.95.197.94/logins/git_callback
const users = require('./routes/api/users')
router.use('/logins', users)
const transer = require('./routes/api/transer')
router.use('/api/transer', transer)

app.use(serve(path.join(__dirname + "/public")))
app.use(bodyParser())
app.use(router.routes())
   .use(router.allowedMethods())

const port = 2399

app.listen(port, () => {
    console.log(`http://localhost:${port}`)
})