describe("My First Test", () => {
  it("Gets, types and asserts", () => {
    cy.visit("http://first-cloud-resume.s3-website-us-east-1.amazonaws.com/");

    cy.contains("Synopsis").should("exist");
    // cy.contains("VisitorsGetCount").should("exist");
    // cy.get("#VisitorsGetCount").should("be.gt", 0);
    // cy.get('[id*="VisitorsGetCount"]').should("be.gt", 0);
    // Should be on a new URL which
    // includes '/commands/actions'
    // cy.contains("#VisitorsGetCount").should("be.gt", 0);
    // cy.reload();

    // // Get an input, type into it
    // cy.get('.action-email').type('fake@email.com')

    // //  Verify that the value has been updated
    // cy.get('.action-email').should('have.value', 'fake@email.com')
  });
});

describe("Returns an updated value from database", () => {
  it("fetches visitorCount", () => {
    cy.request(
      "https://pm4bzwzn80.execute-api.us-east-1.amazonaws.com/Prod/putcount"
    ).then((resp) => {
      const data = resp.body;

      expect(resp.status).to.eq(200);

      expect(data.count).to.not.be.oneOf([null, "", undefined]);
    });
  });
});
