const mongoose = require('mongoose')

const Schema = mongoose.Schema

const todayRequestTimesSchema = new Schema({
    times: {
        type: Number,
        default: 0,
        required: true
    },
    history:{
        type: Array,
        default: []
    }
})

module.exports = mongoose.model('TodayRequestTime', todayRequestTimesSchema)