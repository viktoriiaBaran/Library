const {getData, setData, removeData} = require('./firebase');

async function login(login, password, users, ws) {
    if(Object.keys(users).includes(login)) return 'user_logined';
    let response = '';
    const data = await getData(`Users/${login}`);
    if(data) {
        if(data.password === password) {
            const { name } = data;
            console.log(`User ${name}(${login}) is connected.\n`);
            response = name;
        }
        else {
            response = 'wrong_password'
        }
    }
    else {
        response = 'missing_login';
    }
    return response;
}

async function registration(name, surname, login, password) {
    let response = '';
    if(await getData(`Users/${login}`)) {
        response = 'Користувач з даним логіном вже існує.';
    }
    else {
        let updates = {};
        updates[`Users/${login}/name`] = name[0].toUpperCase() + name.slice(1);
        updates[`Users/${login}/surname`] = surname[0].toUpperCase() + surname.slice(1);
        updates[`Users/${login}/password`] = password;
        await setData(updates);
        response = 'Реєстрація пройшла успішно!';
    }
    return response;
}

async function getBooks() {
    let response = '';
    const books = await getData(`Books`);
    for(let id in books) response += `${id}: ${books[id].name}\n`;
    return response.trim();
}

async function searchBooks(search) {
    let response = '';
    const books = await getData(`Books`);
    for(let id in books) {
        if(/^(\d){4}$/g.test(search)) {
            if(id.toString() === search) response += `${id}: ${books[id].name}\n`;
        }
        else {
            const { name } = books[id];
            if(name.toLowerCase().includes(search.toLowerCase())) response += `${id}: ${name}\n`;
        }
    }
    return response.trim();
}

async function searchBooksSection(search) {
    let response = '';
    const books = await getData(`Books`);
    for(let id in books) {
        const { name, section } = books[id];
        if(section.toLowerCase().includes(search.toLowerCase())) response += `${id}: ${name}\n`;
    }
    return response.trim();
}

async function getBooksInformation(id) {
    let response = '';
    const book = await getData(`Books/${id}`);
    const { name, avtor, section, language, page, about } = book;
    response += `Назва: ${name}\n`;
    response += `Автор: ${avtor}\n`;
    response += `Жанр: ${section}\n`;
    response += `Мова: ${language}\n`;
    response += `Кількість сторінок: ${page}\n`;
    response += `Опис: ${about}`;
    return response;
}


async function getUserInfo(login) {
    const user = await getData(`Users/${login}`);
    const { name, surname } = user;
    return `${name} ${surname}`;
}

async function createOrder(login, book, phone) {
    const key = Math.round(100000 - 0.5 + Math.random() * (999999 - 100000 + 1));
    let updates = {};
    updates[`Users/${login}/Orders/${key}/book`] = book;
    updates[`Users/${login}/Orders/${key}/phone`] = phone;
    await setData(updates);
    return key;
}

async function viewOrders(login) {
    let response = '';
    const orders = await getData(`Users/${login}/Orders`);
    Object.keys(orders).forEach(id => response += `Замовлення-${id}\n`);
    console.log(response);
    return response.trim();
}

async function orderInfo(login, id) {
    let response = '';
    const user = await getData(`Users/${login}`);
    const { name, surname } = user;
    response += `Ім'я: ${name}\n`;
    response += `Прізвище: ${surname}\n`;
    const order = user.Orders[id];
    const { book, phone } = order;
    response += `Номер телефону: ${phone}\n`;
    response += `Книги:\n${book}`;
    return response;
}

async function removeOrder(login, id) {
    removeData(`Users/${login}/Orders/${id}`);
    return 'Замовлення успішно скасовано!'
}

module.exports = {
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
};
