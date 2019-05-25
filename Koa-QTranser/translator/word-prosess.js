const jinshan = require('./jinshan')
const baidu = require('./baidu')

module.exports = async word => {
    let resultJS = await jinshan(word)
    let resultBD = await baidu(word)
    let transResult = {}
        if(resultJS.symbols[0].parts && resultJS.exchange != null) {
            transResult = {
                api: 'jinshan-en',
                en : resultJS.symbols[0].ph_en,
                am : resultJS.symbols[0].ph_am,
                src: resultJS.word_name,
                dst: resultJS.symbols[0].parts,
            }
        } else if(resultJS.symbols[0].parts) {
            let dst = []
            resultJS.symbols[0].parts[0].means.forEach(element => {
                dst.push(element.word_mean)
            })
            transResult = {
                api: 'jinshan-cn',
                en : '',
                am : '',
                src: '',
                dst: dst,
            }
        }
        console.log(resultBD)
        let str = ''
        resultBD.trans_result.forEach(element => {
            str += element.dst + '\n'
        })
    
        transResult.baidu = str

    return transResult
}
