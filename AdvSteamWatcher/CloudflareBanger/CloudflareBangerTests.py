import unittest
import subprocess

class Test_CloudflareBangerTests(unittest.TestCase):
    def test_NoParameters(self):
        output = subprocess.check_output(["py", "CloudflareBanger.py"]).decode().strip('\r\n')
        self.assertEqual("[ERROR] Incorrect arguments amount (expected 1, got 0)", output, "Got invalid error")

    def test_CloudflareSite(self):
        output = str(subprocess.check_output(["py", "CloudflareBanger.py", "https://www.gametracker.rs/sms_boost/"]))
        self.assertIn("html", output, "Missing 'html' string")
        self.assertIn("<", output, "Missing '<'")
        self.assertIn(">", output, "Missing '>'")
        self.assertIn("PayPal boost", output, "Missing 'PayPal boost' string")

    def test_NonCloudflareSite(self):
        output = str(subprocess.check_output(["py", "CloudflareBanger.py", "https://google.com"]))
        self.assertIn("html", output, "Missing 'html' string")
        self.assertIn("<", output, "Missing '<'")
        self.assertIn(">", output, "Missing '>'")
        self.assertIn("apis.google.com", output, "Missing 'apis.google.com' string")
    
    def test_IncorrectURL(self):
        output = subprocess.check_output(["py", "CloudflareBanger.py", "dddddddd"]).decode().strip('\r\n')
        self.assertEqual("[ERROR] Invalid URL 'dddddddd': No schema supplied. Perhaps you meant http://dddddddd?", output)

    def test_TooManyParameters(self):
        output = subprocess.check_output(["py", "CloudflareBanger.py", "https://gametracker.rs", "https://google.com"]).decode().strip('\r\n')
        self.assertEqual("[ERROR] Incorrect arguments amount (expected 1, got 2)", output, "Got invalid error")

if __name__ == '__main__':
    unittest.main()
