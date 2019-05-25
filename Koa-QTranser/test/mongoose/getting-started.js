const mongoose = require('mongoose')

mongoose.connect('mongodb://localhost/test',  { useNewUrlParser: true })

let db = mongoose.connection
db.on('error', console.error.bind(console, 'connection error'))
db.once('open', () => {
    console.log('mongodb connected...')

    let kittySchema = new mongoose.Schema({
        name: String
    })

    kittySchema.methods.speak = function() {
        let greeting = this.name
        ? 'Meow name is ' + this.name
        : "I don't have a name"
        console.log(greeting)
    }

    //let Kitten = mongoose.model('Kitten', kittySchema)
    let Kitten = mongoose.model('Kitten', kittySchema, 'kitten')

    let felyne = new Kitten({ 
        name: 'Felyne'
    })
    console.log(felyne.name)

    let fluffy = new Kitten({ 
        name: 'fluffy'
    })
    fluffy.speak()

    fluffy.save( (err, fluffy) =>{
        if(err) return console.log(err)
        fluffy.speak()
    })
    
    Kitten.find((err, kittens) => {
        if(err) return console.log(err)
        console.log(kittens)
    })

    let connection = mongoose.createConnection('mongodb://localhost:27017/test')
    let Tanku = connection.model('Tanku', kittySchema)

    let xyf = new Tanku({ name: 'xyfll7'})
    xyf.save((err, xyf) => {
        if(err) return console.log(err)
    })
    Tanku.find((err, xyfs) => {
        if(err) return console.log(err)
        console.log(xyfs)
    })

    Tanku.find({ name: 'xyfll7'}).where()
})