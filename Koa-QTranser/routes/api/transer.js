const Router = require('koa-router')
const wordProcess = require('../../translator/word-prosess')
const UserRequestTimes = require('../../models/user-request-times')
const TodayRequestTimes = require('../../models/today-request-times')
const config = require('../../config/config')
const router = new Router()


todayRequestTime = new TodayRequestTimes({
    times: 1,
})


TodayRequestTimes.findById(config.todayRequestTimes_id ,(err, result) => {
    if(err) console.log(err)
    if(!result) {
        todayRequestTime.save().then(requestTime => {
            console.log(requestTime)
        }).catch(err => {
            console.log(err)
        })
    }
})

// 软件升级
router.get('/update', async ctx => {
    ctx.body = config.update
})
// 翻译api
router.post('/', async ctx => {
    ctx.params
    ctx.body = await wordProcess(ctx.request.body.word)

    await TodayRequestTimes.findOneAndUpdate(
        {_id: config.todayRequestTimes_id},
        { $inc: { times: 1 } },
        { new : true }
    )

    let a = await UserRequestTimes.findOneAndUpdate(
        { mac: ctx.request.body.mac },
        { $inc: { times: 1 } },
        { new : true }
    )

    if(a == null) {
        const userRequestTime = new UserRequestTimes({
            mac: ctx.request.body.mac,
            times: 1
        })

        await userRequestTime.save().then(requestTime => {
            console.log(requestTime)
        }).catch(err => {
            console.log(err)
        })
    }
})



module.exports = router.routes()