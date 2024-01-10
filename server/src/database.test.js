const {
    login,
    registration,
    getBooks,
    getBooksInformation,
    getUserInfo,
    createOrder,
    orderInfo,
    removeOrder
} = require('./database');

const { getData, setData, removeData } = require('./firebase');

jest.mock('./firebase', () => ({
    getData: jest.fn(),
    setData: jest.fn(),
    removeData: jest.fn()
}));

describe('login function', () => {
    const users = {
        'user123': { password: 'pass123', name: 'John' }
    };

    test('user exists and enters the correct password', async () => {
        const loginResponse = await login('user123', 'pass123', users, 'ws');
        expect(loginResponse).toBe('user_logined');
    });

    test('user does not exist', async () => {
        getData.mockResolvedValue(null);
        const loginResponse = await login('nonexistent_user', 'password', {}, 'ws');
        expect(loginResponse).toBe('missing_login');
    });
});

describe('registration function', () => {
    test('Registration with non-existing user', async () => {
        getData.mockResolvedValue(null);
    
        const registrationResponse = await registration('John', 'Doe', 'johndoe', 'password123');
    
        expect(setData).toHaveBeenCalledWith({
            'Users/johndoe/name': 'John',
            'Users/johndoe/surname': 'Doe',
            'Users/johndoe/password': 'password123'
        });
        expect(registrationResponse).toBe('Реєстрація пройшла успішно!');
        });
    
        test('Registration with existing user', async () => {
        getData.mockResolvedValue({ name: 'John', surname: 'Doe', password: 'password123' });
    
        const registrationResponse = await registration('Jane', 'Smith', 'johndoe', 'password123');
    
        expect(registrationResponse).toBe('Користувач з даним логіном вже існує.');
    });
});

describe('getBooks function', () => {
    test('Retrieve books information', async () => {
        const booksData = {
            book1: { name: 'Book One' },
            book2: { name: 'Book Two' },
        };
    
        getData.mockResolvedValue(booksData);
    
        const expectedResponse = 'book1: Book One\nbook2: Book Two';
        const booksResponse = await getBooks();
    
        expect(getData).toHaveBeenCalledWith('Books');
        expect(booksResponse).toBe(expectedResponse);
    });
});

describe('getBooksInformation', () => {
    test('book information is shown', async () => {
        const bookData = {
            name: 'Book Name',
            avtor: 'Book Author',
            section: 'Fantasy',
            language: 'English',
            page: 300,
            about: 'Description of the book',
        };
    
        getData.mockResolvedValue(bookData);
    
        const bookInfo = await getBooksInformation('book_id');
    
        expect(getData).toHaveBeenCalledWith('Books/book_id');
        expect(bookInfo).toBe(
            `Назва: ${bookData.name}\n` +
            `Автор: ${bookData.avtor}\n` +
            `Жанр: ${bookData.section}\n` +
            `Мова: ${bookData.language}\n` +
            `Кількість сторінок: ${bookData.page}\n` +
            `Опис: ${bookData.about}`
        );
    });
});

describe('getUserInfo', () => {
    test('is user info shown', async () => {
        const userData = {
            name: 'Vika',
            surname: 'Baran',
        };
    
        getData.mockResolvedValue(userData);
    
        const userInfo = await getUserInfo('xxxagrid');
    
        expect(getData).toHaveBeenCalledWith('Users/xxxagrid');
        expect(userInfo).toBe(`${userData.name} ${userData.surname}`);
    });
});

describe('createOrder function', () => {
    afterEach(() => {
        jest.restoreAllMocks();
    });
    
    test('Create a new order', async () => {
        const login = 'user123';
        const book = 'Book 1';
        const phone = '1234567890';
    
        jest.spyOn(Math, 'random').mockReturnValue(0.5);
    
        const randomKey = Math.round(100000 - 0.5 + Math.random() * (999999 - 100000 + 1));
    
       // const setDataMock = jest.spyOn(setData, 'mockImplementation').mockImplementation();
    
        const createdOrderKey = await createOrder(login, book, phone);
    
        expect(setData).toHaveBeenCalledWith({
            [`Users/${login}/Orders/${randomKey}/book`]: book,
            [`Users/${login}/Orders/${randomKey}/phone`]: phone
        });
        expect(createdOrderKey).toBe(randomKey);
    });
});

describe('orderInfo function', () => {
    test('Display information for a specific order of a user', async () => {
        const orderData = {
            name: 'John',
            surname: 'Doe',
            Orders: {
            order1: { book: 'Book 1', phone: '1234567890' },
            order2: { book: 'Book 2', phone: '9876543210' },
            },
        };
    
        getData.mockResolvedValue(orderData);
    
        const login = 'user123';
        const orderId = 'order1';
        const expectedResponse =
            `Ім'я: John\n` +
            `Прізвище: Doe\n` +
            `Номер телефону: 1234567890\n` +
            `Книги:\nBook 1`;
    
        const orderInfoResponse = await orderInfo(login, orderId);
    
        expect(getData).toHaveBeenCalledWith(`Users/${login}`);
        expect(orderInfoResponse).toBe(expectedResponse);
    });
});

describe('removeOrder function', () => {
    test('Remove an order', async () => {
        const login = 'user123';
        const orderId = 'order123';
    
        const expectedPath = `Users/${login}/Orders/${orderId}`;
    
        const removalResponse = await removeOrder(login, orderId);
    
        expect(removeData).toHaveBeenCalledWith(expectedPath);
        expect(removalResponse).toBe('Замовлення успішно скасовано!');
    });
});
