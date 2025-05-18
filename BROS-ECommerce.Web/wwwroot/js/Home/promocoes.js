const btnLeft = document.getElementById('btn-left');
const btnRight = document.getElementById('btn-right');
const container = document.getElementById('carousel');

const scrollAmount = () => {
    const cards = container.querySelectorAll('.card');
    if (cards.length >= 2) {
        const gap = cards[1].offsetLeft - cards[0].offsetLeft - cards[0].offsetWidth;
        return cards[0].offsetWidth + gap;
    }
    return 300;
};

btnLeft.addEventListener('click', () => {
    container.scrollBy({ left: -scrollAmount(), behavior: 'smooth' });
});

btnRight.addEventListener('click', () => {
    container.scrollBy({ left: scrollAmount(), behavior: 'smooth' });
});