const mongoose = require('mongoose')

const Schema = mongoose.Schema

const  userRequestTimeSchema = new Schema({
    mac: {
        type: String,
        required: true
    },
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

module.exports = mongoose.model('UserRequestTime', userRequestTimeSchema)