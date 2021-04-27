import SimpleStore  from './simpleStore.js'
const api = {
    key: '###',
    querySearch: '',
    pageNum: 1,
};

const refs = {
    list: document.querySelector('#Search-list'),
    form: document.querySelector('#Search-Form'),
    commonWordsList: document.querySelector("#MostCommonWords"),
    input: document.querySelector('input'),
    button: document.querySelector('button'),
};

export class BookDetails {

    topCommonWords = [];
    searchResult = [];

    constructor() {
        this.topCommonWords = [];
        this.searchResult = [];
    }
    
    loadTopCommonWords() {
        refs.commonWordsList.innerHTML = "";
        var bookId = new SimpleStore().getContent().id;
            fetch(
                `/api/books/${bookId}`,
            )
                .then(response => {
                    if (response.ok) return response.json();
                    throw new Error(
                        `${response.status} error while your search has occured`,
                    );
                })
                .then(function (data) {
                    // This is the JSON from our response
                    //var books = response.json();
                    for (let i = 0; i < data.length; i++) {
                        var li = document.createElement("li");
                        var inputValue = data[i].Word_Name + ":" + data[i].Count_Of_Occurance;
                        var t = document.createTextNode(inputValue);
                        li.appendChild(t);
                        document.getElementById("MostCommonWords").appendChild(li);
                    }
                }).catch(function (err) {
                    // There was an error
                    console.warn('Something went wrong.', err);
                });
        
    }
    catchInput() {
        //this.search(refs.input.value);
        refs.list.innerHTML = '';
        var querySearch = refs.input.value;
        var bookId = new SimpleStore().getContent().id;
        if (querySearch.length > 2) {
            fetch(
                `/api/books/${bookId}?query=${querySearch}`,
            )
                .then(response => {
                    if (response.ok) return response.json();
                    throw new Error(
                        `${response.status} error while your search has occured`,
                    );
                })
                .then(function (data) {
                    // This is the JSON from our response
                    //var books = response.json();
                    for (let i = 0; i < data.length; i++) {
                        var li = document.createElement("li");
                        var inputValue = data[i];
                        var t = document.createTextNode(inputValue);
                        li.appendChild(t);
                        document.getElementById("Search-List").appendChild(li);
                    }
                }).catch(function (err) {
                    // There was an error
                    console.warn('Something went wrong.', err);
                });
        }
    }
    //const handleClick = debounce(this.catchInput, 250);
    
    search(querySearch) {
       
    }
   
}


//refs.input.addEventListener('input', _.debounce(catchInput, 700));

//function catchInput() {
//    getResults(refs.input.value);
//}

//function incrementPage() {
//    api.pageNum += 1;
//}

//function getResults(querySearch) {
//    refs.list.innerHTML = '';
//    if (querySearch.length > 0) {
//        fetch(
//            `https://pixabay.com/api/?image_type=photo&orientation=horizontal&q=${querySearch}&page=${api.pageNum}&per_page=12&key=${api.key}`,
//        )
//            .then(response => {
//                if (response.ok) return response.json();
//                throw new Error(
//                    `${response.status} error while your search has occured`,
//                );
//            })
//            .then(pic => {
//                const info = pic.hits.map(item => template(item)).join('');
//                refs.list.insertAdjacentHTML('beforeend', info);
//            })
//            .catch(err => {
//                throw err;
//            });
//        incrementPage();
//    }
//}

//refs.button.addEventListener('click', catchInput);