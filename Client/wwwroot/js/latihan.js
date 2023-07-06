//Set Background for Div => go back to initial color
let part1 = document.getElementById("part1");
part1.style.backgroundColor = "red";

let part2 = document.getElementById("part2");
part2.style.backgroundColor = "blue";

let part3 = document.getElementById("part3");
part3.style.backgroundColor = "green";

//Set Background for Button
let button = document.querySelectorAll(".button");
button[0].style.backgroundColor = "lightcoral";
button[1].style.backgroundColor = "lightblue";
button[2].style.backgroundColor = "lightgreen";

//add event listener for button
button[0].addEventListener("click", () => {
    toggleBackgroundColor(part1, "red", "lightcoral");
})

button[1].addEventListener("click", () => {
    toggleBackgroundColor(part2, "blue", "lightblue");
})

button[2].addEventListener("click", () => {
    toggleBackgroundColor(part3, "green", "lightgreen"); })

//add event listener for div
part1.addEventListener("click", () => {
    part1.style.backgroundColor = "red";
})

part2.addEventListener("click", () => {
    part2.style.backgroundColor = "blue";
})

part3.addEventListener("click", () => {
    part3.style.backgroundColor = "green";
})

//Toggle to turnback the color via button
function toggleBackgroundColor(element, color1, color2) {
    const currentColor = element.style.backgroundColor;
    const targetColor = currentColor === color1 ? color2 : color1;
    element.style.backgroundColor = targetColor;
}

//Change the color when mouse hover to body background
let bodyHtml = document.body;

bodyHtml.addEventListener("mouseenter", setRandomBackgroundColor);
bodyHtml.addEventListener("mouseleave", resetBackgroundColor);

function setRandomBackgroundColor() {
    // Generate random RGB values
    const red = Math.floor(Math.random() * 256);
    const green = Math.floor(Math.random() * 256);
    const blue = Math.floor(Math.random() * 256);

    // Create the RGB color string
    const randomColor = `rgb(${red}, ${green}, ${blue})`;

    // Apply the random color as the background
    bodyHtml.style.backgroundColor = randomColor;
}

function resetBackgroundColor() {
    // Reset the background color to its original state
    bodyHtml.style.backgroundColor = "";
}

