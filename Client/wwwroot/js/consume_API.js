//console.log("Hello World")

//let teks = document.querySelector("p");
//teks.innerHTML = "Hello World";

//Adjust responsive text

//$.ajax({
//    url: "https://pokeapi.co/api/v2/pokemon/"
//}).done((result) => {
//    //console.log(result.results)
//    let temp = ""
//    $.each(result.results, (key, value) => {
//        //console.log(value.name);
//        temp += `<tr>
//                    <td>${key + 1}</td>
//                    <td>${value.name}</td>
//                    <td><button onclick="detail('${value.url}')" data-bs-toggle="modal" data-bs-target="#modalSW" class="btn btn-primary"> Detail </button></td>
//                </tr>
//                `
//    })
//    $("#pokemonSW").html(temp);
//    console.log(result);
//})

const colours = {
    normal: '#A8A77A',
    fire: '#EE8130',
    water: '#6390F0',
    electric: '#F7D02C',
    grass: '#7AC74C',
    ice: '#96D9D6',
    fighting: '#C22E28',
    poison: '#A33EA1',
    ground: '#E2BF65',
    flying: '#A98FF3',
    psychic: '#F95587',
    bug: '#A6B91A',
    rock: '#B6A136',
    ghost: '#735797',
    dragon: '#6F35FC',
    dark: '#705746',
    steel: '#B7B7CE',
    fairy: '#D685AD',
};


let detail = (urlString) => {
    $.ajax({
        url: urlString
    }).done((result) => {
        $("#modal-title").html(result.name);

        //Adding Types Color
        let type = ""
        $.each(result.types, (key, value) => {
            let name = value.type.name
            let color = colours.hasOwnProperty(name) ? colours[name] : null;
            if (color == null) {
                type += `<span class="badge bg-light text-dark">${name}</span>`;
            } else {
                type += `<span class="badge text-dark" style="background-color: ${colours[name]}">${name}</span>`;
            }
            console.log(value.type.name);
        });
        $("#types").html(type);

        //Add Image
        console.log(result.sprites.other["official-artwork"].front_default);

        // Clear the existing content of modal-image
        $("#modal-image").empty();

        // Create an HTML image element
        let image = document.createElement("img");

        // Set the source of the image element
        image.src = result.sprites.other["official-artwork"].front_default;

        // Apply CSS styling to the image for dynamic size
        image.style.maxWidth = "100%";
        image.style.maxHeight = "100%";

        // Append the image element to the modal-image div
        $("#modal-image").append(image);


        //Add Abilities
        let abilities = ""
        $.each(result.abilities, (key, value) => {
            abilities += `<li>${value.ability.name}</li>`
        })

        $("#modal-abilities ul").html(abilities)


        //Add Value on stats
        let hp = result.stats[0].base_stat
        let attack = result.stats[1].base_stat
        let defense = result.stats[2].base_stat
        let special_attack = result.stats[3].base_stat
        let special_defense = result.stats[4].base_stat
        let speed = result.stats[5].base_stat
        function updateProgressBar(progressBarId, percentage) {
            const progressBar = document.getElementById(progressBarId);
            progressBar.style.width = percentage + '%';
            progressBar.setAttribute('aria-valuenow', percentage);

            const progressBarText = document.createElement('span');
            progressBarText.className = 'progress-bar-text';
            progressBarText.textContent = percentage + '%';

            progressBar.innerHTML = '';
            progressBar.appendChild(progressBarText);
        }

        updateProgressBar('base-hp', hp);
        updateProgressBar('base-attack', attack);
        updateProgressBar('base-defense', defense);
        updateProgressBar('base-special-attack', special_attack);
        updateProgressBar('base-special-defense', special_defense);
        updateProgressBar('base-speed', speed);


        console.log(result);
        //console.log(result);
    })

    //console.log(url);
}

$(document).ready(function () {
    let num = 0;
    $('#pokemonTable').DataTable({
        ajax: {
            url: "https://pokeapi.co/api/v2/pokemon/",
            dataType: "JSON",
            dataSrc: "results" //data source -> butuh array of object
        },
        columns: [
            {
                data: "",
                render: function (data, type, row) {
                    num += 1;
                    return num;
                }
            },
            { data: "name" },
            {
                data: '',
                render: function (data, type, row) {
                    return `<button onclick="detail('${row.url}')" data-bs-toggle="modal" data-bs-target="#modalSW" class="btn btn-primary"> Detail </button>`;
                }
            }
        ]
    });
});