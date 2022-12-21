function inclineWord(number, declensionsWord) {
    number = Math.abs(number) % 100;
    let n1 = number % 10;
    if (number > 10 && number < 20) { return declensionsWord[2]; }
    if (n1 > 1 && n1 < 5) { return declensionsWord[1]; }
    if (n1 === 1) { return declensionsWord[0]; }
    return declensionsWord[2];
}