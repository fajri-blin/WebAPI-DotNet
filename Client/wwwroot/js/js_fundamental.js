let arrayMhsObj = [
    { nama: "budi", nim: "a112015", umur: 20, isActive: true, fakultas: { name: "komputer" } },
    { nama: "joko", nim: "a112035", umur: 22, isActive: false, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112020", umur: 21, isActive: true, fakultas: { name: "komputer" } },
    { nama: "herul", nim: "a112032", umur: 25, isActive: true, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112040", umur: 21, isActive: true, fakultas: { name: "komputer" } },
]
console.log(arrayMhsObj);

//buat sebuah variable 'fakultasKomputer' => yang didalamnya hanya berisi object dengan fakultas komputer.
let fakultasKomputer = arrayMhsObj.filter((obj) => obj.fakultas.name == "komputer");
console.log(fakultasKomputer);

//jika 2 angka di nim terakhir adalah lebih dari >= 30, maka buat isactive == false.
let newArrayMhsObj =[]
for (let obj of arrayMhsObj) {
    let newObj = { ...obj }
    let num = parseInt(newObj.nim.slice(-2));
    if (num >= 30) {
        newObj.isActive = false;
    }
    newArrayMhsObj.push(newObj);
}
console.log(newArrayMhsObj);