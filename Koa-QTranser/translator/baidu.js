const MD5 = require('md5')
const fetch = require('node-fetch')
const { baidu } = require('../config/config')

module.exports = async word => {
    let salt = (new Date).getTime()
    let str = baidu.appid + word + salt + baidu.key;

    let lang = isChina(word)

    const params = new URLSearchParams()
    params.append('q', word)
    params.append('appid', baidu.appid)
    params.append('salt', salt)
    params.append('from', 'auto')
    params.append('to', lang)
    params.append('sign', MD5(str))
    
    let result
    await fetch(`${baidu.transUrl}${params}`)
    .then(res => res.json())
    .then(json => {
        result = json
    }).catch(err => {
        console.log('==>' +err)
        console.log(':::::::::::')
    })
    return result
}

function isChina(str){
    if (escape(str).indexOf( "%u" )<0){
        return 'zh'
    } else {
        return 'en'
    }
}