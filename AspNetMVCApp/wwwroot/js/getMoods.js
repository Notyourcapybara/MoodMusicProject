document.addEventListener("DOMContentLoaded", function () {
  const grid = document.querySelector(".mood-grid");

  fetch("/Mood/GetAllMoods")
    .then(res => res.json())
    .then(moods => {
      grid.innerHTML = ""; // 清空原有卡片
      moods.forEach(mood => {
        const card = document.createElement("div");
        card.className = `mood-card ${mood}`;
        card.onclick = () => location.href = `/Mood/Playlist?mood=${mood}`;

        card.innerHTML = `
          <div class="emoji">${getEmoji(mood)}</div>
          <div class="label">${mood.toUpperCase()}</div>
        `;
        grid.appendChild(card);
      });
    })
    .catch(() => {
      grid.innerHTML = "<p>Failed to load moods.</p>";
    });

  function getEmoji(mood) {
    const emojis = {
      happy: "😊",
      sad: "☹️",
      chill: "😌",
      energy: "😃"
    };
    return emojis[mood.toLowerCase()] || "🎧";
  }
});