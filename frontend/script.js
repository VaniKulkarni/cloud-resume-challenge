function getVisitorsCount() {
  fetch("https://pm4bzwzn80.execute-api.us-east-1.amazonaws.com/Prod/putcount")
    .then((response) => response.json())
    .then(
      (data) =>
        (document.getElementById("VisitorsGetCount").innerText = data.count)
    );
}
