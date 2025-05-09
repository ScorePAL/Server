import requests
import json

swagger_url = "http://localhost:8080/swagger/v1/swagger.json"
readme_path = "../README_test.md"
start_marker = "## API routes"

resp = requests.get(swagger_url)
data = resp.json()

lines = [start_marker]
for path, methods in data["paths"].items():
    for method, details in methods.items():
        summary = details.get("summary", "")
        lines.append(f"- `{method.upper()}` `{path}`: {summary}")
lines.append(end_marker)

with open(readme_path, "r") as f:
    content = f.read()

start = content.find(start_marker)
new_content = content[:start] + "\n".join(lines)

with open(readme_path, "w") as f:
    f.write(new_content)