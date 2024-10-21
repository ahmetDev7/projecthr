import unittest
import httpx


# 8 test integrations happy path
# ? test integrations edge cases
class TestItemLines(unittest.TestCase):

    def setUp(self):
        self.admin_token = "a1b2c3d4e5"
        self.reader_token = "f6g7h8i9j0"
        self.headers = {"Content-Type": "application/json", "API_KEY": self.admin_token}
        self.client = httpx.Client(
            base_url="http://localhost:3000/api/v1", headers=self.headers
        )

    def set_reader_key(self):
        self.client.headers["API_KEY"] = self.reader_token

    # PUT as admin
    def test_update_item_line(self):
        body = {
            "id": 1,
            "name": "Tech phone",
            "description": "Tech phone test",
            "created_at": "2022-08-18 07:05:25",
            "updated_at": "2024-05-15 15:44:28",
        }
        response = self.client.put("/item_lines/1", json=body)
        self.assertEqual(response.status_code, httpx.codes.OK)

    # DELETE as admin
    def test_delete_location(self):       
        response = self.client.delete("/item_lines/2")
        self.assertEqual(response.status_code, httpx.codes.OK)

    # GET all as admin
    def test_get_all(self):
        response = self.client.get("/item_lines")
        self.assertEqual(response.status_code, httpx.codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)
        
    
    # GET items from item_line as admin
    def test_get_by_id_items(self):
        response = self.client.get("/item_lines/1/items")
        self.assertEqual(response.status_code, httpx.codes.OK)        
        self.assertGreaterEqual(len(response.json()), 1)

    # GET single as admin
    def test_get_by_id(self):
        response = self.client.get("/item_lines/1")
        self.assertEqual(response.status_code, httpx.codes.OK)
        self.assertIn("id", response.json())

    # GET all as reader
    def test_get_all_as_reader(self):
        self.set_reader_key()
        response = self.client.get("/item_lines")
        self.assertEqual(response.status_code, httpx.codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)
        
    # GET items from item_line as reader
    def test_get_by_id_items_as_reader(self):
        self.set_reader_key()
        response = self.client.get("/item_lines/1/items")
        self.assertEqual(response.status_code, httpx.codes.OK)        
        self.assertGreaterEqual(len(response.json()), 1)

    # GET single as reader
    def test_get_by_id_as_reader(self):
        self.set_reader_key()
        response = self.client.get("/item_lines/1")
        self.assertEqual(response.status_code, httpx.codes.OK)
        self.assertIn("id", response.json())


if __name__ == "__main__":
    unittest.main()
