module.exports = ({startTime, func, islog = false}) =>{
    var end = new Date(new Date(new Date().toLocaleDateString()).getTime()+24*60*60*1000-1)
    let now = Date.now()
    setTimeout(func, end-now + 1000 * 60 * 60 * startTime)

    if(islog) {
        houre = parseInt( (end-now + 1000 * 60 * 60 * startTime)/1000/60/60 )
        minute = parseInt( (end-now + 1000 * 60 * 60 * startTime)/1000/60%60 )
        console.log(`${houre} 小时 ${minute} 分钟以后执行,也就是今天夜里 ${startTime} 点整开始执行`)
    }
}