const ws = require('ws');

const {
  login,
  registration,
  getBooks,
  searchBooks,
  searchBooksSection,
  getBooksInformation,
  getUserInfo,
  createOrder,
  viewOrders,
  orderInfo,
  removeOrder
} = require('./database.js');


const port = process.env.PORT || 3000;
const wss = new ws.Server({
  port,
}, () => console.log(`Server started on ${port}\n`));

let users = {};

wss.on('connection', (ws) => {
  ws.onmessage = async req => {
    let resp = '';
    const data = JSON.parse(req.data);
    if(data.func === 'login') {
      resp = await login(data.login, data.password, users, ws);
    }
    if(data.func === 'registration') {
      resp = await registration(data.name, data.surname, data.login, data.password);
    }
    if(data.func === 'getBooks') {
      resp = await getBooks();
    }
    if(data.func === 'searchBooks') {
      resp = await searchBooks(data.search);
    }
    if(data.func === 'searchBooksSection') {
      resp = await searchBooksSection(data.search);
    }
    if(data.func === 'getUserInformation') {
      resp = await getBooksInformation(data.id);
    }
    if(data.func === 'getUserInfo') {
      resp = await getUserInfo(data.login);
    }
    if(data.func === 'createOrder') {
      resp = await createOrder(data.login, data.book, data.phone);
    }
    if(data.func === 'viewOrders') {
      resp = await viewOrders(data.login);
    }
    if(data.func === 'orderInfo') {
      resp = await orderInfo(data.login, data.id);
    }
    if(data.func === 'removeOrder') {
      resp = await removeOrder(data.login, data.id);
    }

    console.log(output(data)); 
    console.log(`Respond:\n${resp}\n`);
    ws.send(resp);
  };

  ws.onclose = () => {
    const login = getLogin(users, ws);
    if(login) {
      delete users[login];
      console.log(`User ${login} is disconnected.\n`);
    }
  }
});

function output(data) {
  console.log('New request:');
  for(let key in data) {
    if(!data[key]) delete data[key]
  }
  return data;
}

function getLogin(object, value) {
  return Object.keys(object).find(key => object[key] === value);
}