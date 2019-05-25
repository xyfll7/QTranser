const UserStatistics = require('../models/user-request-times')
const TodayRequestTimes = require('../models/today-request-times')
const startTime = require('./start-time')
const config = require('../config/config')

async function addHistory() {
    let result = await UserStatistics.find()

    TodayRequestTimes.findById(config.todayRequestTimes_id ,async (err, result) => {
        if(err) console.log(err)
        if(result) {
            console.log(result)
            let date = new Date()
            await TodayRequestTimes.findOneAndUpdate(
                {
                    '_id': config.todayRequestTimes_id
                },
                {
                    $push: {
                        history: {
                            times: result.times,
                            date: date.toLocaleDateString()
                        }
                    },
                    times: 0 
                },
                {
                    upsert: true
                }
            ) 
        }
    })




    result.forEach(async element => {
        let date = new Date()

        await UserStatistics.findOneAndUpdate(
            {
                '_id': element._id
            },
            {
                $push: {
                    history: {
                        times: element.times,
                        date: date.toLocaleDateString()
                    }
                },
                times: 0
            },
            {
                upsert: true
            }
        )
    });
    console.log(result)
    startTime({startTime: 0, func: addHistory})
}

module.exports = addHistory


