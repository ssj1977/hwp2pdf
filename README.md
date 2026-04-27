# hwp2pdf

한글(HWP/HWPX 등) 문서를 CLI 또는 GUI로 다양한 포맷으로 변환하는 Windows 도구입니다.

## 현재 상태

- 상태 머신: `DOCUMENTING`
- 마지막 검증 일시: `2026-04-27`
- CLI 변환 검증: `PDF/HWPX/HWPML2X/HTML+/OOXML/UNICODE/RTF` 성공, `HWP`는 입력이 동일 확장자라 스킵, `ODT` 실패(원인 미확정)

## 최근 변경 내역

- 2026-04-27: CLI 빌드/실행 환경 이슈(.NET 4.6.1 타기팅 팩, COM 등록) 해결 절차 정리
- 2026-04-27: `test.hwp` 기반 전 포맷 변환 실측 결과 문서화
- 2026-04-27: 전체 포맷 일괄 변환 명령(스크립트) 추가

## 문서 안내 맵

- 제품 요구사항/범위: `docs/PRD.md`
- 시스템 구조: `docs/architecture.md`
- 구현/실행/검증 절차: `docs/implementation.md`
- 문제 원인/해결 기록: `docs/findings.md`
- 의사결정 로그: `docs/decision.md`
- 보안/공급망 정책: `docs/security_policy.md`
- 진행 상태: `docs/progress.md`
- 계획/작업 목록: `docs/plan.md`, `docs/todo.md`
- 프롬프트 및 실행 이력: `docs/prompt_history.md`

## 문서 갱신 상태

- `docs/PRD.md`: 최신
- `docs/security_policy.md`: 최신
- `docs/architecture.md`: 최신
- `docs/implementation.md`: 최신
- `docs/plan.md`: 최신
- `docs/todo.md`: 최신
- `docs/progress.md`: 최신
- `docs/prompt_history.md`: 최신
- `docs/decision.md`: 최신
- `docs/findings.md`: 최신

## 요구사항

- Windows 환경
- .NET Framework `4.6.1` 타기팅 팩(빌드 시)
- 한/글(한컴오피스) 설치 및 COM 자동화 사용 가능 상태

## 빌드

```powershell
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\MSBuild.exe" .\hwp2pdf.csproj /t:Rebuild /p:Configuration=Release /p:Platform=x86
```

## 사용 방법 (Windows Terminal CLI)

```powershell
hwp2pdf.exe C:\docs\a.hwp C:\docs\b.hwpx
```

```powershell
hwp2pdf.exe C:\docs --output C:\out --target PDF --overwrite rename
```

주요 옵션:

- `--input`, `-i`: 입력 파일/폴더 반복 지정
- `--output`, `-o`: 출력 폴더
- `--target`, `-t`: 대상 형식 (`PDF`, `HWP`, `HWPX`, `HWPML2X`, `HTML+`, `ODT`, `OOXML`, `UNICODE`, `RTF`)
- `--overwrite`: 이름 충돌 처리 (`rename`, `skip`, `overwrite`)
- `--pdf-print`: PDF 가상 프린터 방식 사용
- `--printer`: PDF 프린터 이름 지정
- `--print-method`: 인쇄 방식 번호
- `--help`: 도움말
- `--gui`: GUI 모드 실행

실행 기본 동작:

- 인자 없음: GUI 실행
- 인자 있음: CLI 실행

## 모든 포맷 일괄 변환 명령

전체 명령은 `docs/implementation.md`의 `모든 포맷 일괄 변환` 섹션에 있습니다.

## 기타

접근 권한 경고창이 나타나면 변환 시작 시 `모두 허용`을 선택해야 정상 동작합니다.
