import { A4demoPage } from './app.po';

describe('a4demo App', () => {
  let page: A4demoPage;

  beforeEach(() => {
    page = new A4demoPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
