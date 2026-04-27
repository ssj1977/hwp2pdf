# Architecture

## 구성 요소

- `Program.cs`
  - 인자 유무로 GUI/CLI 진입점 분기
- `CliRunner.cs`
  - CLI 옵션 파싱
  - 입력 파일 수집
  - 대상 포맷 변환 실행
  - 충돌 파일 처리(`rename/skip/overwrite`)
- `FormMain.cs` 및 관련 Form 파일
  - 기존 GUI 기능

## 외부 의존성

- .NET Framework `4.6.1`
- 한글 COM 자동화 객체 (`HWPFrame.HwpObject`)
- 한글 자동화 타입 라이브러리 GUID:
  - TypeLib: `{7D2B6F3C-1D95-4E0C-BF5A-5EE564186FBC}`
  - CLSID 확인 대상: `{2291CF00-64A1-4877-A9B4-68CFE89612D6}`

## 실행 흐름 (CLI)

1. `Program.Main`에서 CLI 실행 여부 판단
2. `CliRunner.TryParse`로 옵션 파싱
3. 입력 파일 목록 수집
4. `HwpObject` 생성 및 보안 모듈 등록 시도
5. 대상 포맷별 `SaveAs` 또는 PDF 인쇄 경로 실행
6. 성공/실패/스킵 요약 출력
