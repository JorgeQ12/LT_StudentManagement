import { GlobalLoadingService } from './global-loading.service';

describe('GlobalLoadingService', () => {
  it('keeps the loader active until every concurrent request finishes', () => {
    const service = new GlobalLoadingService();

    service.begin();
    service.begin();
    expect(service.isLoading()).toBe(true);

    service.end();
    expect(service.isLoading()).toBe(true);

    service.end();
    expect(service.isLoading()).toBe(false);
  });

  it('never decrements below zero', () => {
    const service = new GlobalLoadingService();

    service.end();
    expect(service.isLoading()).toBe(false);
  });
});
