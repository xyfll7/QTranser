const { URLSearchParams  } = require('url')
const fetch = require('node-fetch')
const { jinshan } = require('../config/config')

const params = new URLSearchParams()
params.append('type', 'json')
params.append('key', jinshan.key)

module.exports = async word => {
    let result = ''
    params.append('w', word)
    await fetch(`${jinshan.transUrl}${params}`)
    .then(res => res.json())
    .then(json => {
        result = json
    }).catch(err => {
        console.log('==>' +err)
        console.log(':::::::::::')
    })
    return result
}
