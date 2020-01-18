
import cloudscraper
scraper = cloudscraper.create_scraper()
print("start")
print(scraper.get("https://gametracker.rs").content)
print("done")
