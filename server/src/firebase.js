
const { initializeApp } = require('firebase/app');
const { getDatabase, ref, child, get, update, remove } = require ('firebase/database');

const firebaseConfig = {
  apiKey: "AIzaSyBStlRXd79pAyEcy3yudXYL4XzEbuoSy_0",
  authDomain: "ipz-server-780ee.firebaseapp.com",
  databaseURL: "https://library-db32c-default-rtdb.europe-west1.firebasedatabase.app/",
  projectId: "ipz-server-780ee",
  storageBucket: "ipz-server-780ee.appspot.com",
  messagingSenderId: "861001538718",
  appId: "1:861001538718:web:4e8ec748626c0d757daf8f"
};

const app = initializeApp(firebaseConfig);

const dbRef = ref(getDatabase(app));

async function getData(path) {
  return await get(child(dbRef, path)).then(data => data.exists() ? data.val() : '');
}
  
async function setData(updates) {
  return await update(dbRef, updates).then(() => true);
}

async function removeData(path) {
  const databaseRef = ref(getDatabase(app), path);
  return await remove(databaseRef).then(() => true);
}

module.exports = { getData, setData, removeData };
