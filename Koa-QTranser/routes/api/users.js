const Router = require('koa-router')
const fetch = require('node-fetch')
const url = require('url')


const router = new Router()



const client_id = '2bad7ade7264aaafee4b'

const client_secret = '8adb979d9ff45a8001244da440099d2db9e564cd' 

router.get('/git_callback', async ctx => {
    const code = ctx.query.code
    let access_token = ''
    await fetch(`https://github.com/login/oauth/access_token?client_id=${client_id}&client_secret=${client_secret}&code=${code}`)
    .then(res => res.text() )
    .then(search => {
        searchParams = new url.URLSearchParams(search)
        access_token = searchParams.get('access_token')
    })

    ctx.body = ` 
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Document</title>
</head>
<body>
    
</body>
    <script>
    async function a(){
        await fetch('http://localhost:2399/github?access_token=${access_token}')
        .then(res => res.text())
        .then(text => {
            console.log(text)
        })
        window.location.href="about:blank";
        window.close();
    }
    a()
    </script>
</html>`

    // await fetch(`https://api.github.com/user?access_token=${access_token}`)
    // .then(res => res.json())
    // .then(json => {
	//     ctx.body = json
    // })
})
module.exports = router.routes()
