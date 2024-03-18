describe("S3 statis website should block public access", () => {
  it("Gets, types and asserts", () => {
    cy.visit(
      "http://vani.kulkarnisworklife.uk.s3-website-us-east-1.amazonaws.com"
    );

    cy.contains("403 Forbidden").should("exist");
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

describe("Cloudflare should be able redirect to s3 website", () => {
  it("Gets, types and asserts", () => {
    cy.visit("https://vani.kulkarnisworklife.uk");

    cy.contains("Synopsis").should("exist");
    cy.contains("Visitor Count: ").should("exist");
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
